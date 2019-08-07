using Gateway.Contracts.Public.Models;
using Refit;
using System;
using System.Threading.Tasks;

namespace Gateway.Client.Interfaces
{
    public interface IGatewayHttpClient
    {
        [Get("/api/payments")]
        Task<PaymentResponse> GetPaymentAsync(Guid paymentId, [Header("Authorization")] string authorization);

        [Post("/api/payments")]
        Task<PaymentResponse> CreatePaymentAsync(PaymentRequest request, [Header("Authorization")] string authorization);
    }
}