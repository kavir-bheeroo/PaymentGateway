namespace Gateway.Contracts.Models
{
    public class PaymentResponseModel
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
        public CardModel Card { get; set; }
    }
}