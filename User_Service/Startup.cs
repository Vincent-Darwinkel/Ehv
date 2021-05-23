using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Data;
using System.Text.Json.Serialization;
using User_Service.Dal;
using User_Service.Dal.Interfaces;
using User_Service.Logic;
using User_Service.Models.HelperFiles;
using User_Service.RabbitMq;
using User_Service.RabbitMq.Publishers;
using User_Service.RabbitMq.Rpc;

namespace User_Service
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
                throw new NoNullAllowedException();
            }

            services.AddDbContextPool<DataContext>(
                dbContextOptions => dbContextOptions
                                        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddControllers();
            AddDependencies(ref services);
        }

        public void AddDependencies(ref IServiceCollection services)
        {
            IConfigurationSection section = _config.GetSection(nameof(RabbitMqConfig));
            RabbitMqConfig rabbitMqConfig = section.Get<RabbitMqConfig>();

            services.AddScoped<ControllerHelper>();
            services.AddScoped<IPublisher, Publisher>();

            services.AddSingleton(service => new RabbitMqChannel(rabbitMqConfig).GetChannel());
            services.AddSingleton(service => AutoMapperConfig.Config.CreateMapper());

            services.AddScoped<UserLogic>();
            services.AddScoped<LogLogic>();
            services.AddScoped<JwtLogic>();
            services.AddScoped<DisabledUserLogic>();
            services.AddScoped<ActivationLogic>();
            services.AddScoped<DisabledUserLogic>();
            services.AddScoped<IRpcClient, RpcClient>();

            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IActivationDal, ActivationDal>();
            services.AddScoped<IDisabledUserDal, DisabledUserDal>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var channel = app.ApplicationServices.GetService<IModel>();
            var userLogic = app.ApplicationServices.GetService<UserLogic>();
            var disabledUserLogic = app.ApplicationServices.GetService<DisabledUserLogic>();
            var logLogic = app.ApplicationServices.GetService<LogLogic>();

            // ReSharper disable once ObjectCreationAsStatement
            new RpcServer(channel, RabbitMqQueues.FindUserQueue, userLogic.Find, logLogic);
            new RpcServer(channel, RabbitMqQueues.DisabledExistsUserQueue, disabledUserLogic.Exists, logLogic);

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

            UpdateDatabase(app);
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.Migrate();
        }
    }
}
