using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MerchantSimulator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.UseUrls("http://locahost:7001")
                .UseStartup<Startup>();
    }
}