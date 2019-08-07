using CorrelationId;
using Gateway.Client.Handlers;
using Gateway.Client.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;

namespace Gateway.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGatewayHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CorrelationIdOptions>(configuration.GetSection("CorrelationId"));

            services.AddTransient<ValidateAuthorizationHeaderHandler>();
            services.AddTransient<CorrelationIdHandler>();

            services.AddHttpClient("gatewayClient", c =>
            {
                c.BaseAddress = new Uri(configuration["Gateway:BaseUrl"]);
            })
            .AddTypedClient(c => Refit.RestService.For<IGatewayHttpClient>(c))
            .AddHttpMessageHandler<ValidateAuthorizationHeaderHandler>()
            .AddHttpMessageHandler<CorrelationIdHandler>()
            .AddTransientHttpErrorPolicy(p =>
                p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));
        }
    }
}