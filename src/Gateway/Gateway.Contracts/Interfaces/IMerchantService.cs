using Gateway.Contracts.Models;
using System.Threading.Tasks;

namespace Gateway.Contracts.Interfaces
{
    public interface IMerchantService
    {
        Task<MerchantModel> GetMerchantModelBySecretKeyAsync(string secretKey);
    }
}