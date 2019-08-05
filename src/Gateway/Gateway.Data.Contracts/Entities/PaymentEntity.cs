using System;

namespace Gateway.Data.Contracts.Entities
{
    public class PaymentEntity
    {
        public Guid PaymentId { get; set; }
        public int MerchantId { get; set; }
        public string MerchantName { get; set; }
        public int AcquirerId { get; set; }
        public string AcquirerName { get; set; }
        public string Status { get; set; }
        public string ResponseCode { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CardholderName { get; set; }
        public string AcquirerPaymentId { get; set; }
        public string AcquirerStatus { get; set; }
        public string AcquirerResponseCode { get; set; }
        public DateTime PaymentTime { get; set; }
    }
}