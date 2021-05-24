using System;
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
            string[] files = Directory.GetFiles(Environment.CurrentDirectory);
            foreach (var file in files)
            {
                Console.WriteLine("File: " + file);
            }
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
                            .AddJsonFile("config/appsettings.Kubernetes.json", true)
                            .AddJsonFile("ocelot.Kubernetes.json", true)
                            .AddJsonFile("ocelot.Development.json", true)
                            .AddEnvironmentVariables();
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}