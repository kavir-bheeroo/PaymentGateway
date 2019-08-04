namespace Gateway.Acquiring.Contracts.Models
{
    public class ProcessorRequest
    {
        public string PaymentId { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
        public AcquirerDetails AcquirerDetails { get; set; }
    }
}