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
            throw new NotImplementedException();
        }
    }
}