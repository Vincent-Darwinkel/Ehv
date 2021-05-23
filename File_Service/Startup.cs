using File_Service.Logic;
using File_Service.Models.HelperFiles;
using File_Service.RabbitMq;
using File_Service.RabbitMq.Publishers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            IConfigurationSection section = _config.GetSection(nameof(RabbitMqConfig));
            RabbitMqConfig rabbitMqConfig = section.Get<RabbitMqConfig>();

            services.AddSingleton(service => new RabbitMqChannel(rabbitMqConfig).GetChannel());
            services.AddScoped<IPublisher, Publisher>();
            services.AddScoped<FileLogic>();
            services.AddScoped<LogLogic>();
            services.AddScoped<JwtLogic>();
            services.AddScoped<DirectoryLogic>();
            services.AddScoped<VirusScannerLogic>();
            services.AddScoped<FileHelper>();
            services.AddScoped<ControllerHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
        }
    }
}
