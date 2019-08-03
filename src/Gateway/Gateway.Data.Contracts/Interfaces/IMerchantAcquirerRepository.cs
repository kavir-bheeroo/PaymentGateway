using Gateway.Data.Contracts.Entities;
using System.Threading.Tasks;

namespace Gateway.Data.Contracts.Interfaces
{
    public interface IMerchantAcquirerRepository
    {
        Task<MerchantAcquirerEntity> GetMerchantAcquirerByMerchantIdAsync(int merchantId);
    }
}