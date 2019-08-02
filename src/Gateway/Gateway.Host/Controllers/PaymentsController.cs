using Gateway.Common;
using Gateway.Common.Extensions;
using Gateway.Contracts.Interfaces;
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
        private readonly IMerchantService _merchantService;

        public PaymentsController(IMerchantService merchantService)
        {
            _merchantService = Guard.IsNotNull(merchantService, nameof(merchantService));
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> Post(PaymentRequest request)
        {
            var merchantId = User.GetMerchantId();
            var merchantName = User.GetMerchantName();
            //var merchant = 
            //var model = _mapper.Map<CheckMrzStatusRequestModel>(request);
            //var result = await _kycService.CheckMrzTaskStatusAsync(model);
            //var response = _mapper.Map<CheckMrzStatusResponse>(result);

            return Ok(merchantName);
        }
    }
}