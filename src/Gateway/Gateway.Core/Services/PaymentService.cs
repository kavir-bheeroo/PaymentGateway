using Gateway.Common;
using Gateway.Contracts.Interfaces;
using Gateway.Contracts.Models;
using Gateway.Data.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMerchantAcquirerRepository _merchantAcquirerRepository;

        public PaymentService(IMerchantAcquirerRepository merchantAcquirerRepository)
        {
            _merchantAcquirerRepository = Guard.IsNotNull(merchantAcquirerRepository, nameof(merchantAcquirerRepository));
        }

        public Task<PaymentResponseModel> ProcessPaymentAsync(PaymentRequestModel request, MerchantModel merchant)
        {
            Guard.IsNotNull(request, nameof(request));

            throw new NotImplementedException();
        }
    }
}