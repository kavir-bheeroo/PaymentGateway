using Microsoft.Extensions.DependencyInjection;
using Npgsql;
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
    }
}