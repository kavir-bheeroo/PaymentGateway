using Gateway.Data.Contracts.Entities;
using System.Threading.Tasks;

namespace Gateway.Data.Contracts.Interfaces
{
    public interface IMerchantRepository
    {
        Task<MerchantEntity> GetBySecretKeyAsync(string secretKey);
    }
}