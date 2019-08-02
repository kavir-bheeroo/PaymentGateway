using Gateway.Contracts.Interfaces;
using Gateway.Core.Services;
using Gateway.Data.Contracts.Interfaces;
using Gateway.Data.Dapper;
using Gateway.Data.Dapper.Repositories;
using Gateway.Host.Authentication;
using Gateway.Host.Extensions;
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

            services.Configure<DatabaseOptions>(Configuration.GetSection(DatabaseOptions.DefaultSectionName));
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IMerchantService, MerchantService>();

            services.AddAuthentication(SecretKeyAuthenticationDefaults.AuthenticationScheme)
                .AddSecretKey(); 

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}