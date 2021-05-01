using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Logging_Service.Dal;
using Logging_Service.Dal.Interfaces;
using Logging_Service.Logic;
using Logging_Service.Models.Helpers;
using Logging_Service.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Logging_Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<DataContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(ServerVersion.AutoDetect(connectionString)));

            services.AddControllers();
            AddDependencies(ref services);
        }

        public void AddDependencies(ref IServiceCollection services)
        {
            services.AddScoped(service => new RabbitMqChannel().GetChannel());
            services.AddSingleton(service => AutoMapperConfig.Config.CreateMapper());
            services.AddScoped<LogLogic>();
            services.AddScoped<ILogDal, LogDal>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            DataContext context = app.ApplicationServices.GetService<DataContext>();
            ApplyMigrations(context);
        }

        public void ApplyMigrations(DataContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
