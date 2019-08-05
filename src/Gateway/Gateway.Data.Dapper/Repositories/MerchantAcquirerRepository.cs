using Dapper;
using Gateway.Common;
using Gateway.Data.Contracts.Entities;
using Gateway.Data.Contracts.Interfaces;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Threading.Tasks;

namespace Gateway.Data.Dapper.Repositories
{
    public class MerchantAcquirerRepository : IMerchantAcquirerRepository
    {
        private readonly DatabaseOptions _databaseOptions;

        public MerchantAcquirerRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = Guard.IsNotNull(databaseOptions, nameof(databaseOptions)).Value;
        }

        public async Task<MerchantAcquirerEntity> GetByMerchantIdAsync(int merchantId)
        {
            Guard.IsNotNull(merchantId, nameof(merchantId));

            const string query = @"
                SELECT	ma.id,
                        ma.merchant_id,
                        ma.acquirer_id,
                        m.name merchant_name,
                        m.secret_key,
                        a.name acquirer_name,
                        a.url
                FROM	merchant_acquirers ma
                INNER JOIN merchants m ON m.id = ma.merchant_id
                INNER JOIN acquirers a ON a.id = ma.acquirer_id
                WHERE	ma.merchant_id = @merchantId
            ";

            var parameters = new { merchantId };

            using (var connection = new NpgsqlConnection(_databaseOptions.GatewayDatabaseConnectionString))
            {
                var entity = await connection.QueryFirstOrDefaultAsync<MerchantAcquirerEntity>(query, parameters);
                return entity;
            }
        }
    }
}