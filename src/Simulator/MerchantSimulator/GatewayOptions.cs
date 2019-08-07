namespace MerchantSimulator
{
    public class GatewayOptions
    {
        public const string DefaultSectionName = "Gateway";

        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }
}