using Autofac;
using Gateway.Data.Contracts.Interfaces;
using Gateway.Data.Dapper.Repositories;

namespace Gateway.Data.Dapper
{
    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<MerchantRepository>()
                .As<IMerchantRepository>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<MerchantAcquirerRepository>()
                .As<IMerchantAcquirerRepository>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<AcquirerResponseCodeMappingRepository>()
                .As<IAcquirerResponseCodeMappingRepository>()
                .InstancePerLifetimeScope();
        }
    }
}