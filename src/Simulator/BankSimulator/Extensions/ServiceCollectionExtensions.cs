using CorrelationId;
using Gateway.Common.Logging.Serilog.Enrichers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BankSimulator.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLogger(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
            var correlationContextAccessor = serviceProvider.GetRequiredService<ICorrelationContextAccessor>();
            var correlationIdOptions = configuration.GetSection("CorrelationId").Get<CorrelationIdOptions>();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", configuration["ApplicationName"])
                .Enrich.With(new CorrelationIdEnricher(correlationContextAccessor, correlationIdOptions))
                .WriteTo.Seq(configuration["Seq:ServerUrl"])
                .CreateLogger();
        }
    }
}