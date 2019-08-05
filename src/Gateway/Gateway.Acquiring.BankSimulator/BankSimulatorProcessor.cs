using AutoMapper;
using Gateway.Acquiring.BankSimulator.Models;
using Gateway.Acquiring.Contracts.Interfaces;
using Gateway.Acquiring.Contracts.Models;
using Gateway.Common;
using Gateway.Common.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Acquiring.BankSimulator
{
    public class BankSimulatorProcessor : IProcessor
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public BankSimulatorProcessor(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = Guard.IsNotNull(httpClientFactory, nameof(httpClientFactory));
            _mapper = Guard.IsNotNull(mapper, nameof(mapper));
        }
        public async Task<ProcessorResponse> ProcessPaymentAsync(ProcessorRequest request)
        {
            Guard.IsNotNull(request, nameof(request));

            var bankSimulatorRequest = _mapper.Map<BankSimulatorRequest>(request);

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var payload = JsonConvert.SerializeObject(bankSimulatorRequest);
                var requestContent = new StringContent(payload, Encoding.UTF8, "application/json");

                using (var httpResponse = await httpClient.PostAsync(new Uri(request.AcquirerDetails.Url), requestContent))
                {
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        throw new GatewayException($"Response from acquirer '{ request.AcquirerDetails.Name }' was not successful.");
                    }

                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    var bankSimulatorResponse = JsonConvert.DeserializeObject<BankSimulatorResponse>(responseContent);

                    var response = _mapper.Map<ProcessorResponse>(bankSimulatorResponse);
                    return response;
                }
            }
        }
    }
}