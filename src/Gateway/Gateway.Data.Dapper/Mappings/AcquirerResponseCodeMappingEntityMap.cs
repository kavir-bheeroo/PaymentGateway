using Dapper.FluentMap.Mapping;
using Gateway.Data.Contracts.Entities;

namespace Gateway.Data.Dapper.Mappings
{
    public class AcquirerResponseCodeMappingEntityMap : EntityMap<AcquirerResponseCodeMapping>
    {
        public AcquirerResponseCodeMappingEntityMap()
        {
            Map(m => m.Id).ToColumn("id");
            Map(m => m.AcquirerId).ToColumn("acquirer_id");
            Map(m => m.AcquirerResponseCode).ToColumn("acquirer_response_code");
            Map(m => m.GatewayResponseCode).ToColumn("gateway_response_code");
        }
    }
}