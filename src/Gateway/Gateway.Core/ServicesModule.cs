using Autofac;
using FluentValidation;
using Gateway.Acquiring.BankSimulator;
using Gateway.Acquiring.Contracts;
using Gateway.Acquiring.Contracts.Interfaces;
using Gateway.Contracts.Interfaces;
using Gateway.Contracts.Models;
using Gateway.Core.Security;
using Gateway.Core.Services;
using Gateway.Core.Validators;

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
                .RegisterType<AesCryptor>()
                .As<ICryptor>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<PaymentRequestModelValidator>()
                .As<IValidator<PaymentRequestModel>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<CardRequestModelValidator>()
                .As<IValidator<CardRequestModel>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<MerchantModelValidator>()
                .As<IValidator<MerchantModel>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<BankSimulatorProcessor>()
                .Keyed<IProcessor>(ProcessorList.BankSimulator);
        }
    }
}