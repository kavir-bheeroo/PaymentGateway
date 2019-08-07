namespace Gateway.Contracts.Public.Models
{
    public class PaymentRequest
    {
        public string TrackId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public CardRequest Card { get; set; }
    }
}