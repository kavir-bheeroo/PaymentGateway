using AutoMapper;
using Gateway.Common;
using Gateway.Contracts.Interfaces;
using Gateway.Contracts.Models;
using Gateway.Contracts.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using CustomResponse = Gateway.Common.Web.Models.WebResponse;

namespace Gateway.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _mapper = Guard.IsNotNull(mapper, nameof(mapper));
            _paymentService = Guard.IsNotNull(paymentService, nameof(paymentService));
        }

        /// <summary>
        /// Get a Payment By Id
        /// </summary>
        [HttpGet("{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentResponse>> Get(Guid paymentId)
        {
            var merchant = _mapper.Map<MerchantModel>(User);
            var result = await _paymentService.GetByIdAsync(paymentId, merchant);
            var response = _mapper.Map<PaymentResponse>(result);

            return Ok(response);
        }

        /// <summary>
        /// Process a new payment.
        /// </summary>
        /// <param name="request">The payment request details.</param>
        /// <returns>The payment response details.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentResponse>> Post(PaymentRequest request)
        {
            var paymentRequest = _mapper.Map<PaymentRequestModel>(request);
            var merchant = _mapper.Map<MerchantModel>(User);

            var result = await _paymentService.ProcessAsync(paymentRequest, merchant);
            var response = _mapper.Map<PaymentResponse>(result);

            return CreatedAtAction(nameof(Get), new { paymentId = response.PaymentId }, response);
        }
    }
}