using Autofac;
using Gateway.Acquiring.BankSimulator;
using Gateway.Acquiring.Contracts;
using Gateway.Acquiring.Contracts.Interfaces;
using Gateway.Contracts.Interfaces;
using Gateway.Core.Services;

namespace Gateway.Core
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<PaymentService>()
                .As<IPaymentService>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<MerchantService>()
                .As<IMerchantService>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<BankSimulatorProcessor>()
                .Keyed<IProcessor>(ProcessorList.BankSimulator);
        }
    }
}