using BankSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BankSimulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        [HttpPost]
        public ActionResult<PaymentResponse> Post(PaymentRequest request)
        {
            var response = new PaymentResponse
            {
                PaymentId = Guid.NewGuid().ToString(),
                GatewayPaymentId = request.GatewayPaymentId,
                Amount = request.Amount,
                Currency = request.Currency,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                CardholderName = request.CardholderName,
                CardNumber = request.CardNumber
            };

            switch (request.Amount)
            {
                case 1000:
                    response.Status = "Failed";
                    response.ResponseCode = "400";
                    break;

                default:
                    response.Status = "Succeeded";
                    response.ResponseCode = "200";
                    break;
            }

            return Ok(response);
        }
    }
}