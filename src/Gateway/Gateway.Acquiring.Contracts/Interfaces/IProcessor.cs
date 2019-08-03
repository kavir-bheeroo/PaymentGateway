using Gateway.Acquiring.Contracts.Models;
using System.Threading.Tasks;

namespace Gateway.Acquiring.Contracts.Interfaces
{
    public interface IProcessor
    {
        Task<ProcessorResponse> ProcessPaymentAsync(ProcessorRequest request);
    }
}