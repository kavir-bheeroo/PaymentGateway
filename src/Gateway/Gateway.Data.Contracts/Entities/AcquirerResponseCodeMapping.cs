namespace Gateway.Data.Contracts.Entities
{
    public class AcquirerResponseCodeMapping
    {
        public int Id { get; set; }
        public int AcquirerId { get; set; }
        public string AcquirerResponseCode { get; set; }
        public string GatewayResponseCode { get; set; }
    }
}