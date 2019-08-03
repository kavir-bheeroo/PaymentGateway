using Gateway.Acquiring.Contracts.Interfaces;
using Gateway.Acquiring.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace Gateway.Acquiring.BankSimulator
{
    public class BankSimulatorProcessor : IProcessor
    {
        public Task<ProcessorResponse> ProcessPaymentAsync(ProcessorRequest request)
        {
            return Task.FromResult(new ProcessorResponse { AcquirerPaymentId = Guid.NewGuid().ToString(), PaymentId = request.PaymentId });
        }
    }
}