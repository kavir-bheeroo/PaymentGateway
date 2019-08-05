namespace Gateway.Contracts.Models
{
    public class CardResponseModel
    {
        public string Number { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Name { get; set; }
    }
}