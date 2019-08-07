using App.Metrics.AspNetCore;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Gateway.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.UseUrls("http://localhost:6001")
                .UseMetrics()
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>();
    }
}