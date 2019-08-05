using Autofac.Features.Indexed;
using AutoMapper;
using Gateway.Acquiring.Contracts;
using Gateway.Acquiring.Contracts.Interfaces;
using Gateway.Acquiring.Contracts.Models;
using Gateway.Common;
using Gateway.Common.Exceptions;
using Gateway.Contracts.Interfaces;
using Gateway.Contracts.Models;
using Gateway.Data.Contracts.Entities;
using Gateway.Data.Contracts.Interfaces;
using System;
using System.Threading.Tasks;

namespace Gateway.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMerchantAcquirerRepository _merchantAcquirerRepository;
        private readonly IAcquirerResponseCodeMappingRepository _acquirerResponseCodeMappingRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IIndex<ProcessorList, IProcessor> _processors;
        private readonly IMapper _mapper;

        private IProcessor _processor;

        public PaymentService(
            IMerchantAcquirerRepository merchantAcquirerRepository,
            IAcquirerResponseCodeMappingRepository acquirerResponseCodeMappingRepository,
            IPaymentRepository paymentRepository,
            IIndex<ProcessorList, IProcessor> processors,
            IMapper mapper)
        {
            _merchantAcquirerRepository = Guard.IsNotNull(merchantAcquirerRepository, nameof(merchantAcquirerRepository));
            _acquirerResponseCodeMappingRepository = Guard.IsNotNull(acquirerResponseCodeMappingRepository, nameof(acquirerResponseCodeMappingRepository));
            _paymentRepository = Guard.IsNotNull(paymentRepository, nameof(paymentRepository));
            _processors = Guard.IsNotNull(processors, nameof(processors));
            _mapper = Guard.IsNotNull(mapper, nameof(mapper));
        }

        public async Task<PaymentResponseModel> ProcessPaymentAsync(PaymentRequestModel request, MerchantModel merchant)
        {
            Guard.IsNotNull(request, nameof(request));
            Guard.IsNotNull(merchant, nameof(merchant));

            var merchantAcquirer = await _merchantAcquirerRepository.GetByMerchantIdAsync(merchant.Id);

            if (merchantAcquirer == null)
            {
                throw new GatewayException($"No acquirer setup for merchant '{ merchant.Name }'.");
            }

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

            var processingResponse = await _processor.ProcessPaymentAsync(processorRequest);
            var responseCodeMapping = await _acquirerResponseCodeMappingRepository.GetByAcquirerResponseCodeAsync(processingResponse.AcquirerResponseCode);

            var response = _mapper.Map<PaymentResponseModel>(processingResponse);
            response.ResponseCode = responseCodeMapping?.GatewayResponseCode ?? Constants.FailedResponseCode;

            // Map response details to the payment and update data store
            _mapper.Map(response, payment);
            await _paymentRepository.UpdateAsync(payment);

            return response;
        }

        private PaymentEntity BuildPaymentEntity(PaymentRequestModel request, MerchantAcquirerEntity merchantAcquirer)
        {
            return new PaymentEntity
            {
                PaymentId = Guid.NewGuid(),
                MerchantId = merchantAcquirer.MerchantId,
                MerchantName = merchantAcquirer.MerchantName,
                AcquirerId = merchantAcquirer.AcquirerId,
                AcquirerName = merchantAcquirer.AcquirerName,
                Status = Constants.PendingStatus,
                Amount = request.Amount,
                Currency = request.Currency,
                CardNumber = request.Card.Number,
                ExpiryMonth = request.Card.ExpiryMonth,
                ExpiryYear = request.Card.ExpiryYear,
                CardholderName = request.Card.Number,
                PaymentTime = DateTime.UtcNow
            };
        }
    }
}