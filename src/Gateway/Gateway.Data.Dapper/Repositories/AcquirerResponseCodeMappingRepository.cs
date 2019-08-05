using Dapper;
using Gateway.Common;
using Gateway.Data.Contracts.Entities;
using Gateway.Data.Contracts.Interfaces;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Threading.Tasks;

namespace Gateway.Data.Dapper.Repositories
{
    public class AcquirerResponseCodeMappingRepository : IAcquirerResponseCodeMappingRepository
    {
        private readonly DatabaseOptions _databaseOptions;

        public AcquirerResponseCodeMappingRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = Guard.IsNotNull(databaseOptions, nameof(databaseOptions)).Value;
        }

        public async Task<AcquirerResponseCodeMapping> GetMappingByAcquirerResponseCodeAsync(string acquirerResponseCode)
        {
            Guard.IsNotNull(acquirerResponseCode, nameof(acquirerResponseCode));

            const string query = @"
                SELECT	id,
                        acquirer_id,
                        acquirer_response_code,
                        gateway_response_code
                FROM	acquirer_response_code_mappings
                WHERE	acquirer_response_code = @acquirerResponseCode
            ";

            var parameters = new { acquirerResponseCode };

            using (var connection = new NpgsqlConnection(_databaseOptions.GatewayDatabaseConnectionString))
            {
                var entity = await connection.QueryFirstOrDefaultAsync<AcquirerResponseCodeMapping>(query, parameters);
                return entity;
            }
        }
    }
}
