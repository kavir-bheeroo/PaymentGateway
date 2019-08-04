﻿namespace BankSimulator.Models
{
    public class PaymentResponse
    {
        public string PaymentId { get; set; }
        public string GatewayPaymentId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CardholderName { get; set; }
        public string Status { get; set; }
        public string ResponseCode { get; set; }
    }
}