namespace Gateway.Acquiring.Contracts.Models
{
    public class ProcessorResponse
    {
        public string AcquirerPaymentId { get; set; }
        public string PaymentId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Number { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Name { get; set; }
        public string AcquirerStatus { get; set; }
        public string AcquirerResponseCode { get; set; }
    }
}