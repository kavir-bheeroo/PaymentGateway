namespace Gateway.Contracts.Models
{
    public class PaymentResponseModel
    {
        public string AcquirerPaymentId { get; set; }
        public string PaymentId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public CardModel Card { get; set; }
    }
}