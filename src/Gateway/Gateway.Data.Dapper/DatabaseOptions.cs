namespace Gateway.Data.Dapper
{
    public class DatabaseOptions
    {
        public const string DefaultSectionName = "Database";

        public string GatewayDatabaseConnectionString { get; set; }
    }
}