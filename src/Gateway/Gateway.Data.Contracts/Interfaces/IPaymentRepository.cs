using Gateway.Data.Contracts.Entities;
using System;
using System.Threading.Tasks;

namespace Gateway.Data.Contracts.Interfaces
{
    public interface IPaymentRepository
    {
        Task<PaymentEntity> GetByIdAsync(Guid id);
        Task<PaymentEntity> InsertAsync(PaymentEntity payment);
        Task UpdateAsync(PaymentEntity payment);
    }
}