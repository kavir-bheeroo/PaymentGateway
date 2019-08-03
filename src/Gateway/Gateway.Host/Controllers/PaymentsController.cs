using AutoMapper;
using Gateway.Common;
using Gateway.Contracts.Interfaces;
using Gateway.Contracts.Models;
using Gateway.Contracts.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> Post(PaymentRequest request)
        {
            var paymentRequestModel = _mapper.Map<PaymentRequestModel>(request);
            var merchantModel = _mapper.Map<MerchantModel>(User);

            var result = await _paymentService.ProcessPaymentAsync(paymentRequestModel, merchantModel);
            var response = _mapper.Map<PaymentResponse>(result);

            return Ok(response);
        }
    }
}