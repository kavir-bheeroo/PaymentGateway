namespace Gateway.Acquiring.Contracts.Models
{
    public class ProcessorResponse
    {
        public string PaymentId { get; set; }
        public string AcquirerPaymentId { get; set; }
        public string AcquirerStatus { get; set; }
        public string AcquirerResponseCode { get; set; }

        public PaymentDetails PaymentDetails { get; set; }
    }
}