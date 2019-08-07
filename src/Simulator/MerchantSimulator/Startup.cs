using CorrelationId;
using Gateway.Client.Extensions;
using Gateway.Common.Logging.Serilog.Extensions;
using Gateway.Common.Web.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace MerchantSimulator
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
            services.Configure<GatewayOptions>(Configuration.GetSection(GatewayOptions.DefaultSectionName));

            services.AddGatewayHttpClient(Configuration);

            services.AddLogger(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog(Logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ResponseMiddleware>();

            var correlationIdOptions = Configuration.GetSection("CorrelationId").Get<CorrelationIdOptions>();
            app.UseCorrelationId(correlationIdOptions);

            app.UseMvc();
        }
    }
}