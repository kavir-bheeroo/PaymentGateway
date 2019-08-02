using Gateway.Common;
using Gateway.Contracts.Interfaces;
using Gateway.Contracts.Models;
using Gateway.Data.Contracts.Interfaces;
using System.Threading.Tasks;

namespace Gateway.Core.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IMerchantRepository _repository;

        public MerchantService(IMerchantRepository repository)
        {
            _repository = Guard.IsNotNull(repository, nameof(repository));
        }

        public async Task<MerchantModel> GetMerchantModelBySecretKeyAsync(string secretKey)
        {
            Guard.IsNotNull(secretKey, nameof(secretKey));

            var entity = await _repository.GetMerchantBySecretKeyAsync(secretKey);

            return new MerchantModel { Id = entity.Id, Name = entity.Name, SecretKey = entity.SecretKey };
        }
    }
}