using Gateway.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace Gateway.Contracts.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseModel> GetByIdAsync(Guid paymentId, MerchantModel merchant);
        Task<PaymentResponseModel> ProcessAsync(PaymentRequestModel request, MerchantModel merchant);
    }
}