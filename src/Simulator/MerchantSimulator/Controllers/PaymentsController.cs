using Gateway.Client.Interfaces;
using Gateway.Common;
using Gateway.Common.Exceptions;
using Gateway.Contracts.Public.Models;
using MerchantSimulator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Refit;
using System;
using System.Net;
using System.Threading.Tasks;
using CustomWebResponse = Gateway.Common.Web.Models.WebResponse;


namespace MerchantSimulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IGatewayHttpClient _httpClient;
        private readonly GatewayOptions _gatewayOptions;

        public PaymentsController(IGatewayHttpClient httpClient, IOptions<GatewayOptions> gatewayOptions)
        {
            _httpClient = Guard.IsNotNull(httpClient, nameof(httpClient));
            _gatewayOptions = Guard.IsNotNull(gatewayOptions, nameof(gatewayOptions)).Value;
        }

        [HttpGet("{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomWebResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CustomWebResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentResponse>> Get(Guid paymentId)
        {
            try
            {
                var response = await _httpClient.GetPaymentAsync(paymentId, _gatewayOptions.ApiKey);
                return Ok(response);
            }
            catch (ApiException ex)
            {
                // Exception handling logic should be moved somewhere else to keep method clean.
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new BadRequestException(ex.Message);
                }

                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException(ex.Message);
                }

                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ObjectNotFoundException(ex.Message);
                }

                throw new GatewayException(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomWebResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentResponse>> Post(MerchantPaymentRequest request)
        {
            var paymentRequest = new PaymentRequest
            {
                TrackId = Guid.NewGuid().ToString(),
                Amount = request.Amount,
                Currency = request.Currency,
                Card = new CardRequest
                {
                    Name = "Merchant Test User",
                    Number = "4953089013607",
                    ExpiryMonth = 11,
                    ExpiryYear = 2022,
                    Cvv = "123"
                }
            };

            try
            {
                var response = await _httpClient.CreatePaymentAsync(paymentRequest, _gatewayOptions.ApiKey);
                return Ok(response);
            }
            catch (ApiException ex)
            {
                // Exception handling logic should be moved somewhere else to keep method clean.

                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new BadRequestException(ex.Message);
                }

                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException(ex.Message);
                }

                throw new GatewayException(ex.Message);
            }
        }
    }
}