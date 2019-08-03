namespace Gateway.Acquiring.Contracts.Models
{
    public class ProcessorRequest
    {
        public string PaymentId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Number { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; set; }
        public string Name { get; set; }
    }
}