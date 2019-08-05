using Gateway.Data.Contracts.Entities;
using System.Threading.Tasks;

namespace Gateway.Data.Contracts.Interfaces
{
    public interface IAcquirerResponseCodeMappingRepository
    {
        Task<AcquirerResponseCodeMapping> GetMappingByAcquirerResponseCodeAsync(string acquirerResponseCode);
    }
}