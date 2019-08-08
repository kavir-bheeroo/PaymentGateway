using Autofac.Features.Indexed;
using AutoMapper;
using Gateway.Acquiring.Contracts;
using Gateway.Acquiring.Contracts.Interfaces;
using Gateway.Acquiring.Contracts.Models;
using Gateway.Common;
using Gateway.Common.Exceptions;
using Gateway.Contracts.Interfaces;
using Gateway.Contracts.Models;
using Gateway.Core.Security;
using Gateway.Data.Contracts.Entities;
using Gateway.Data.Contracts.Interfaces;
using System;
using System.Threading.Tasks;using FluentValidation;

namespace Gateway.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMerchantAcquirerRepository _merchantAcquirerRepository;
        private readonly IAcquirerResponseCodeMappingRepository _acquirerResponseCodeMappingRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IIndex<ProcessorList, IProcessor> _processors;
        private readonly ICryptor _cryptor;
        private readonly IValidator<PaymentRequestModel> _paymentRequestModelValidator;
        private readonly IValidator<MerchantModel> _merchantModelValidator;
        private readonly IMapper _mapper;

        private IProcessor _processor;

        public PaymentService(
            IMerchantAcquirerRepository merchantAcquirerRepository,
            IAcquirerResponseCodeMappingRepository acquirerResponseCodeMappingRepository,
            IPaymentRepository paymentRepository,
            IIndex<ProcessorList, IProcessor> processors,
            ICryptor cryptor,
            IValidator<PaymentRequestModel> paymentRequestModelValidator,
            IValidator<MerchantModel> merchantModelValidator,
            IMapper mapper)
        {
            _merchantAcquirerRepository = Guard.IsNotNull(merchantAcquirerRepository, nameof(merchantAcquirerRepository));
            _acquirerResponseCodeMappingRepository = Guard.IsNotNull(acquirerResponseCodeMappingRepository, nameof(acquirerResponseCodeMappingRepository));
            _paymentRepository = Guard.IsNotNull(paymentRepository, nameof(paymentRepository));
            _processors = Guard.IsNotNull(processors, nameof(processors));
            _cryptor = Guard.IsNotNull(cryptor, nameof(cryptor));
            _paymentRequestModelValidator = Guard.IsNotNull(paymentRequestModelValidator, nameof(paymentRequestModelValidator));
            _merchantModelValidator = Guard.IsNotNull(merchantModelValidator, nameof(merchantModelValidator));
            _mapper = Guard.IsNotNull(mapper, nameof(mapper));
        }

        public async Task<PaymentResponseModel> GetByIdAsync(Guid paymentId, MerchantModel merchant)
        {
            Guard.IsNotNull(paymentId, nameof(paymentId));
            _merchantModelValidator.ValidateAndThrow(merchant);

            var payment = await _paymentRepository.GetByIdAsync(paymentId);

            if (payment == null)
            {
                throw new ObjectNotFoundException($"No payment with id '{ paymentId } was found.'");
            }

            // This check should normally return a Not Found exception for security purposes. Unauthorized used for testing purposes only.
            if (payment.MerchantId != merchant.Id)
            {
                throw new UnauthorizedException($"Merchant is not authorised to retrieve this payment.");
            }

            var response = _mapper.Map<PaymentResponseModel>(payment);
            response.Card.Number = MaskHelper.MaskCardNumber(_cryptor.Decrypt(response.Card.Number));

            return response;
        }

        public async Task<PaymentResponseModel> ProcessAsync(PaymentRequestModel request, MerchantModel merchant)
        {
            _paymentRequestModelValidator.ValidateAndThrow(request);
            _merchantModelValidator.ValidateAndThrow(merchant);

            var merchantAcquirer = await _merchantAcquirerRepository.GetByMerchantIdAsync(merchant.Id);

            if (merchantAcquirer == null)
            {
                throw new GatewayException($"No acquirer setup for merchant '{ merchant.Name }'.");
            }

            // Get the appropriate processor from the AcquirerName.
            var enumAsString = (ProcessorList) Enum.Parse(typeof(ProcessorList), merchantAcquirer.AcquirerName);
            _processor = _processors[enumAsString];

            // Create and insert payment details
            var payment = BuildPaymentEntity(request, merchantAcquirer);
            await _paymentRepository.InsertAsync(payment);

            // Create processor request
            var processorRequest = new ProcessorRequest
            {
                PaymentId = payment.PaymentId.ToString(),
                PaymentDetails = _mapper.Map<PaymentDetails>(request),
                AcquirerDetails = _mapper.Map<AcquirerDetails>(merchantAcquirer)
            };

            var processorResponse = await _processor.ProcessPaymentAsync(processorRequest);
            var responseCodeMapping = await _acquirerResponseCodeMappingRepository.GetByAcquirerResponseCodeAsync(processorResponse.AcquirerResponseCode);

            var response = _mapper.Map<PaymentResponseModel>(processorResponse);
            response.TrackId = request.TrackId;
            response.ResponseCode = responseCodeMapping?.GatewayResponseCode ?? Constants.FailResponseCode;
            response.Status = response.ResponseCode.Equals(Constants.SuccessResponseCode) ? Constants.SuccessStatus : Constants.FailStatus;
            response.Card.Number = MaskHelper.MaskCardNumber(response.Card.Number);

            // Map response details to the payment and update data store
            _mapper.Map(response, payment);
            await _paymentRepository.UpdateAsync(payment);

            return response;
        }

        private PaymentEntity BuildPaymentEntity(PaymentRequestModel request, MerchantAcquirerEntity merchantAcquirer)
        {
            return new PaymentEntity
            {
                PaymentId = Guid.NewGuid().ToString(),
                MerchantId = merchantAcquirer.MerchantId,
                MerchantName = merchantAcquirer.MerchantName,
                AcquirerId = merchantAcquirer.AcquirerId,
                AcquirerName = merchantAcquirer.AcquirerName,
                TrackId = request.TrackId,
                Status = Constants.PendingStatus,
                Amount = request.Amount,
                Currency = request.Currency,
                CardNumber = _cryptor.Encrypt(request.Card.Number),
                ExpiryMonth = request.Card.ExpiryMonth,
                ExpiryYear = request.Card.ExpiryYear,
                CardholderName = request.Card.Name,
                PaymentTime = DateTime.UtcNow
            };
        }
    }
}