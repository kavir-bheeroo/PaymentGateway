namespace Gateway.Contracts.Models
{
    public class PaymentResponseModel
    {
        public string AcquirerPaymentId { get; set; }
        public string PaymentId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public CardModel Card { get; set; }
        public string Status { get; set; }
        public string ResponseCode { get; set; }
        public string AcquirerStatus { get; set; }
        public string AcquirerResponseCode { get; set; }
    }
}