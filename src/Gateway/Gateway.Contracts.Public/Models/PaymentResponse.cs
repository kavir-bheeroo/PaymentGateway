namespace Gateway.Contracts.Public.Models
{
    public class PaymentResponse
    {
        public string AcquirerPaymentId { get; set; }
        public string PaymentId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public Card Card { get; set; }
    }
}