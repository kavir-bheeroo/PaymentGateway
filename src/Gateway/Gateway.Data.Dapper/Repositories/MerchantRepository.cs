using Dapper;
using Gateway.Common;
using Gateway.Data.Contracts.Entities;
using Gateway.Data.Contracts.Interfaces;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Threading.Tasks;

namespace Gateway.Data.Dapper.Repositories
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly DatabaseOptions _databaseOptions;

        public MerchantRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = Guard.IsNotNull(databaseOptions, nameof(databaseOptions)).Value;
        }

        public async Task<MerchantEntity> GetMerchantBySecretKeyAsync(string secretKey)
        {
            const string query = @"
                SELECT	id,
                        name,
                        secret_key
                FROM	merchants
                WHERE	secret_key = @secretKey
            ";

            var parameters = new { secretKey };

            using (var connection = new NpgsqlConnection(_databaseOptions.GatewayDatabaseConnectionString))
            {
                var entity = await connection.QueryFirstOrDefaultAsync<MerchantEntity>(query, parameters);
                return entity;
            }
        }
    }
}