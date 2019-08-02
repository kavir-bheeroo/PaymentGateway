using Gateway.Common;
using Gateway.Contracts.Interfaces;
using Gateway.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Core.Services
{
    public class PaymentService : IPaymentService
    {
        public PaymentService()
        {

        }

        public Task<PaymentResponseModel> ProcessPaymentAsync(PaymentRequestModel request)
        {
            Guard.IsNotNull(request, nameof(request));

            throw new NotImplementedException();
        }
    }
}