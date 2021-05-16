using Event_Service.Dal;
using Event_Service.Dal.Interfaces;
using Event_Service.Logic;
using Event_Service.Models.HelperFiles;
using Event_Service.RabbitMq;
using Event_Service.RabbitMq.Consumers;
using Event_Service.RabbitMq.Publishers;
using Event_Service.RabbitMq.Rpc;
using Event_Service.RabbitMq.RPC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Data;
using System.Text.Json.Serialization;

namespace Event_Service
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
            services.AddScoped<IPublisher, Publisher>();
            services.AddScoped(service => new RabbitMqChannel().GetChannel());
            services.AddSingleton(service => AutoMapperConfig.Config.CreateMapper());
            services.AddScoped<RpcClient>();
            services.AddScoped<ControllerHelper>();

            services.AddScoped<IEventDal, EventDal>();
            services.AddScoped<IEventDateUserDal, EventDateUserDal>();
            services.AddScoped<IEventStepDal, EventStepDal>();
            services.AddScoped<IEventStepUserDal, EventStepUserDal>();

            services.AddScoped<EventLogic>();
            services.AddScoped<JwtLogic>();
            services.AddScoped<EventDateUserLogic>();
            services.AddScoped<EventStepUserLogic>();
            services.AddScoped<EventDateUserLogic>();
            services.AddScoped<LogLogic>();
            services.AddScoped<ConvertToEventConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            new List<IConsumer>
            {
                app.ApplicationServices.GetService<ConvertToEventConsumer>(),
            }.ForEach(consumer => consumer.Consume());

            var channel = app.ApplicationServices.GetService<IModel>();
            var eventLogic = app.ApplicationServices.GetService<EventLogic>();
            var logLogic = app.ApplicationServices.GetService<LogLogic>();

            // ReSharper disable once ObjectCreationAsStatement
            new RpcServer(channel, RabbitMqQueues.ExistsEventQueue, eventLogic.Exists, logLogic);

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
