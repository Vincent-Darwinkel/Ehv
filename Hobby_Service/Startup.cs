using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using Hobby_Service.Dal;
using Hobby_Service.Dal.Interfaces;
using Hobby_Service.Logic;
using Hobby_Service.Models.Helpers;
using Hobby_Service.RabbitMq;
using Hobby_Service.RabbitMq.Publishers;
using Hobby_Service.RabbitMq.Rpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Hobby_Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new NoNullAllowedException();
            }

            services.AddDbContextPool<DataContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            AddDependencies(ref services);
            services.AddControllers();
        }

        public void AddDependencies(ref IServiceCollection services)
        {
            services.AddSingleton(service => AutoMapperConfig.Config.CreateMapper());
            services.AddScoped<LogLogic>();
            services.AddScoped<IHobbyDal, HobbyDal>();
            services.AddScoped<HobbyLogic>();
            services.AddScoped<JwtLogic>();
            services.AddSingleton(service => new RabbitMqChannel().GetChannel());
            services.AddScoped<IPublisher, Publisher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var channel = app.ApplicationServices.GetService<IModel>();
            var hobbyLogic = app.ApplicationServices.GetService<HobbyLogic>();
            var logLogic = app.ApplicationServices.GetService<LogLogic>();

            // ReSharper disable once ObjectCreationAsStatement
            new RpcServer(channel, RabbitMqQueues.AllHobbyQueue, hobbyLogic.AllRabbitMq, logLogic);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}