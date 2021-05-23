using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Gateway_Service
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((builder) =>
                    {
                        builder.SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.Development.json", true)
                            .AddJsonFile("config/appsettings.json", true)
                            .AddJsonFile("ocelot.json", true, false)
                            .AddJsonFile("config/ocelot.json", true)
                            .AddEnvironmentVariables();
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}