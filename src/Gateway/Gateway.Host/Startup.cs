using Autofac;
using AutoMapper;
using Dapper.FluentMap;
using Gateway.Common.Web.Middlewares;
using Gateway.Core;
using Gateway.Core.Security;
using Gateway.Data.Dapper;
using Gateway.Data.Dapper.Mappings;
using Gateway.Host.Authentication;
using Gateway.Host.Extensions;
using Gateway.Host.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.MigrateDatabase(Configuration["Database:GatewayDatabaseConnectionString"]);

            FluentMapper.Initialize(config => {
                config.AddMap(new MerchantEntityMap());
                config.AddMap(new MerchantAcquirerEntityMap());
                config.AddMap(new AcquirerResponseCodeMappingEntityMap());
                config.AddMap(new PaymentEntityMap());
            });

            services.Configure<DatabaseOptions>(Configuration.GetSection(DatabaseOptions.DefaultSectionName));
            services.Configure<SecurityOptions>(Configuration.GetSection(SecurityOptions.DefaultSectionName));

            services.AddAuthentication(SecretKeyAuthenticationDefaults.AuthenticationScheme)
                .AddSecretKey();

            services.AddAutoMapper(
                typeof(MappingProfile),
                typeof(Core.Mappers.MappingProfile),
                typeof(Acquiring.BankSimulator.Mappers.MappingProfile));

            services.AddHttpClient();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ServicesModule());
            builder.RegisterModule(new RepositoriesModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMiddleware<ResponseMiddleware>();
            app.UseMvc();
        }
    }
}