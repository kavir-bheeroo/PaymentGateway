using Dapper.FluentMap.Mapping;
using Gateway.Data.Contracts.Entities;

namespace Gateway.Data.Dapper.Mappings
{
    public class MerchantAcquirerEntityMap : EntityMap<MerchantAcquirerEntity>
    {
        public MerchantAcquirerEntityMap()
        {
            Map(m => m.Id).ToColumn("id");
            Map(m => m.MerchantId).ToColumn("merchant_id");
            Map(m => m.AcquirerId).ToColumn("acquirer_id");
            Map(m => m.MerchantName).ToColumn("merchant_name");
            Map(m => m.SecretKey).ToColumn("secret_key");
            Map(m => m.AcquirerName).ToColumn("acquirer_name");
            Map(m => m.Url).ToColumn("url");
        }
    }
}