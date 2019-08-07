namespace Gateway.Contracts.Models
{
    public class PaymentRequestModel
    {
        public string TrackId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public CardRequestModel Card { get; set; }
    }
}