namespace Gateway.Contracts.Public.Models
{
    public class PaymentResponse
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
        public Card Card { get; set; }
    }
}