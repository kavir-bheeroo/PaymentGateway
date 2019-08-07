using CorrelationId;
using Gateway.Common.Logging.Serilog.Enrichers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Serilog;
using System;

namespace Gateway.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void MigrateDatabase(this IServiceCollection services, string connectionString)
        {
            var connection = new NpgsqlConnection(connectionString);

            var evolve = new Evolve.Evolve(connection, x => Console.WriteLine(x))
            {
                Locations = new[] { "Scripts" }
            };

            evolve.Migrate();
        }

        public static void AddLogger(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
            var correlationContextAccessor = serviceProvider.GetRequiredService<ICorrelationContextAccessor>();
            var correlationIdOptions = configuration.GetSection("CorrelationId").Get<CorrelationIdOptions>();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.With(new CorrelationIdEnricher(correlationContextAccessor, correlationIdOptions))
                .WriteTo.Seq(configuration["Seq:ServerUrl"])
                .CreateLogger();
        }
    }
}