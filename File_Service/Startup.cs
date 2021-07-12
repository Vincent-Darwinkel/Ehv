using File_Service.Logic;
using File_Service.Models.HelperFiles;
using File_Service.RabbitMq;
using File_Service.RabbitMq.Consumers;
using File_Service.RabbitMq.Publishers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using File_Service.Dal;
using Microsoft.EntityFrameworkCore;

namespace File_Service
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = _config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new NoNullAllowedException("Connectionstring is empty");
            }

            services.AddDbContextPool<DataContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddControllers();

            // allow big files to be uploaded
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = 26214400; // max allowed size for clamav
                x.MultipartBodyLengthLimit = 26214400;
            });

            AddDependencies(ref services);
        }

        public void AddDependencies(ref IServiceCollection services)
        {
            IConfigurationSection rabbitMqSection = _config.GetSection(nameof(RabbitMqConfig));

            services.AddSingleton(service => new RabbitMqChannel(rabbitMqSection.Get<RabbitMqConfig>()).GetChannel());
            services.AddScoped<IPublisher, Publisher>();
            services.AddScoped<FileLogic>();
            services.AddScoped<LogLogic>();
            services.AddScoped<JwtLogic>();
            services.AddScoped<DirectoryLogic>();
            services.AddScoped<VirusScannerLogic>();
            services.AddScoped<FileHelper>();
            services.AddScoped<DeleteUserFilesConsumer>();
            services.AddScoped<ControllerHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            new List<IConsumer>
            {
                app.ApplicationServices.GetService<DeleteUserFilesConsumer>()
            }.ForEach(consumer => consumer.Consume());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Console.WriteLine("Install required apt packages via apt install, this can take a while");
            SystemHelper.ExecuteOsCommand("apt-get update && apt-get upgrade -y");
            SystemHelper.ExecuteOsCommand("apt-get install -y apt-utils");
            SystemHelper.ExecuteOsCommand("apt-get install software-properties-common -y");
            SystemHelper.ExecuteOsCommand("apt install -y ffmpeg");
            SystemHelper.ExecuteOsCommand("add-apt-repository ppa:deadsnakes/ppa");
            SystemHelper.ExecuteOsCommand("apt-get update");
            SystemHelper.ExecuteOsCommand("apt-get install -y python3");
            SystemHelper.ExecuteOsCommand("apt-get install -y python3-pip");
            SystemHelper.ExecuteOsCommand("python3 -m pip install --upgrade pip");
            SystemHelper.ExecuteOsCommand("python3 -m pip install --upgrade Pillow");
            Console.WriteLine("Finished installing required packages");
        }
    }
}
