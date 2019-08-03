using Gateway.Contracts.Models;
using System.Threading.Tasks;

namespace Gateway.Contracts.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseModel> ProcessPaymentAsync(PaymentRequestModel request, MerchantModel merchant);
    }
}