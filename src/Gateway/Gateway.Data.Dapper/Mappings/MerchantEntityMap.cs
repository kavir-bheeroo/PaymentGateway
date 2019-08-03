using Dapper.FluentMap.Mapping;
using Gateway.Data.Contracts.Entities;

namespace Gateway.Data.Dapper.Mappings
{
    public class MerchantEntityMap : EntityMap<MerchantEntity>
    {
        public MerchantEntityMap()
        {
            Map(m => m.Id).ToColumn("id");
            Map(m => m.Name).ToColumn("name");
            Map(m => m.SecretKey).ToColumn("secret_key");
        }
    }
}