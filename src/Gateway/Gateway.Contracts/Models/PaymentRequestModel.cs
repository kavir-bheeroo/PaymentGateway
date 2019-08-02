namespace Gateway.Contracts.Models
{
    public class PaymentRequestModel
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
        public CardModel Card { get; set; }
    }
}