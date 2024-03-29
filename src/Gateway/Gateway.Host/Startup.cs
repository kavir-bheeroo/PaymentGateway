﻿using Autofac;
using AutoMapper;
using CorrelationId;
using Dapper.FluentMap;
using FluentValidation.AspNetCore;
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
using Microsoft.Extensions.Logging;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using ILogger = Serilog.ILogger;

namespace Gateway.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }

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
            services.Configure<CorrelationIdOptions>(Configuration.GetSection("CorrelationId"));

            services.AddAuthentication(SecretKeyAuthenticationDefaults.AuthenticationScheme)
                .AddSecretKey();

            services.AddAutoMapper(
                typeof(MappingProfile),
                typeof(Core.Mappers.MappingProfile),
                typeof(Acquiring.BankSimulator.Mappers.MappingProfile));

            services.AddHttpClient();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Gateway", Version = "v1" });
            });

            services.AddCorrelationId();
            services.AddLogger(Configuration);

            services.AddMvc()
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Startup>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMetrics();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ServicesModule());
            builder.RegisterModule(new RepositoriesModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var correlationIdOptions = Configuration.GetSection("CorrelationId").Get<CorrelationIdOptions>();
            app.UseCorrelationId(correlationIdOptions);

            loggerFactory.AddSerilog(Logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMiddleware<ResponseMiddleware>();
            app.UseMiddleware<ValidationMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway V1");
            });

            app.UseMvc();
        }
    }
}