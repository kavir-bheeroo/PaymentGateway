using Autofac.Features.Indexed;
using AutoMapper;
using Gateway.Acquiring.Contracts;
using Gateway.Acquiring.Contracts.Interfaces;
using Gateway.Acquiring.Contracts.Models;
using Gateway.Common;
using Gateway.Contracts.Interfaces;
using Gateway.Contracts.Models;
using Gateway.Data.Contracts.Interfaces;
using System;
using System.Threading.Tasks;

namespace Gateway.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMerchantAcquirerRepository _merchantAcquirerRepository;
        private readonly IAcquirerResponseCodeMappingRepository _acquirerResponseCodeMappingRepository;
        private readonly IIndex<ProcessorList, IProcessor> _processors;
        private readonly IMapper _mapper;
        
        private IProcessor _processor;

        public PaymentService(
            IMerchantAcquirerRepository merchantAcquirerRepository,
            IAcquirerResponseCodeMappingRepository acquirerResponseCodeMappingRepository,
            IIndex<ProcessorList, IProcessor> processors,
            IMapper mapper)
        {
            _merchantAcquirerRepository = Guard.IsNotNull(merchantAcquirerRepository, nameof(merchantAcquirerRepository));
            _acquirerResponseCodeMappingRepository = Guard.IsNotNull(acquirerResponseCodeMappingRepository, nameof(acquirerResponseCodeMappingRepository));
            _processors = Guard.IsNotNull(processors, nameof(processors));
            _mapper = Guard.IsNotNull(mapper, nameof(mapper));
        }

        public async Task<PaymentResponseModel> ProcessPaymentAsync(PaymentRequestModel request, MerchantModel merchant)
        {
            Guard.IsNotNull(request, nameof(request));

            var merchantAcquirer = await _merchantAcquirerRepository.GetMerchantAcquirerByMerchantIdAsync(merchant.Id);

            if (merchantAcquirer == null)
            {
                throw new ArgumentException($"No acquirer setup for merchant '{ merchant.Name }'.");
            }

            var enumAsString = (ProcessorList) Enum.Parse(typeof(ProcessorList), merchantAcquirer.AcquirerName);
            _processor = _processors[enumAsString];

            // Create processor request
            var processorRequest = new ProcessorRequest
            {
                PaymentId = Guid.NewGuid().ToString(),
                PaymentDetails = _mapper.Map<PaymentDetails>(request),
                AcquirerDetails = _mapper.Map<AcquirerDetails>(merchantAcquirer)
            };

            var processingResponse = await _processor.ProcessPaymentAsync(processorRequest);
            var responseCodeMapping = await _acquirerResponseCodeMappingRepository.GetMappingByAcquirerResponseCodeAsync(processingResponse.AcquirerResponseCode);

            var response = _mapper.Map<PaymentResponseModel>(processingResponse);
            response.ResponseCode = responseCodeMapping?.GatewayResponseCode ?? Constants.FailedResponseCode;

            return response;
        }
    }
}