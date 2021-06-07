using Datepicker_Service.Dal;
using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.Logic;
using Datepicker_Service.Models.HelperFiles;
using Datepicker_Service.RabbitMq;
using Datepicker_Service.RabbitMq.Consumers;
using Datepicker_Service.RabbitMq.Publishers;
using Datepicker_Service.RabbitMq.Rpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Data;
using System.Text.Json.Serialization;

namespace Datepicker_Service
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
            IConfigurationSection rabbitMqSection = _config.GetSection(nameof(RabbitMqConfig));

            services.AddScoped<IPublisher, Publisher>();
            services.AddScoped<ControllerHelper>();
            services.AddScoped(service => new RabbitMqChannel(rabbitMqSection.Get<RabbitMqConfig>()).GetChannel());
            services.AddScoped<JwtLogic>();
            services.AddScoped<LogLogic>();
            services.AddScoped<DatepickerLogic>();
            services.AddScoped<DatepickerAvailabilityLogic>();
            services.AddScoped<DeleteUserConsumer>();
            services.AddScoped<IRpcClient, RpcClient>();
            services.AddScoped<IDatepickerDal, DatepickerDal>();
            services.AddScoped<IDatepickerDateDal, DatepickerDateDal>();
            services.AddScoped<IDatepickerAvailabilityDal, DatepickerAvailabilityDal>();
            services.AddSingleton(service => AutoMapperConfig.Config.CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            new List<IConsumer>
            {
                app.ApplicationServices.GetService<DeleteUserConsumer>()
            }.ForEach(consumer => consumer.Consume());

            app.UseRouting();

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
