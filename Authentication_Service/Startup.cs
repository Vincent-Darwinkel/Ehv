using Authentication_Service.Dal;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Logic;
using Authentication_Service.Models.HelperFiles;
using Authentication_Service.RabbitMq;
using Authentication_Service.RabbitMq.Consumers;
using Authentication_Service.RabbitMq.Publishers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Data;
using System.Text.Json.Serialization;
using Authentication_Service.RabbitMq.Rpc;
using DataContext = Authentication_Service.Dal.DataContext;
using IUserDal = Authentication_Service.Dal.Interface.IUserDal;
using UserDal = Authentication_Service.Dal.UserDal;

namespace Authentication_Service
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
            AddDependencyInjection(ref services);
        }

        private void AddDependencyInjection(ref IServiceCollection services)
        {
            IConfigurationSection section = _config.GetSection(nameof(JwtConfig));

            services.AddSingleton(section.Get<JwtConfig>());
            services.AddScoped<UserLogic>();
            services.AddScoped<AuthenticationLogic>();
            services.AddScoped<SecurityLogic>();
            services.AddScoped<JwtLogic>();
            services.AddSingleton(service => new RabbitMqChannel().GetChannel());
            services.AddScoped<AddUserConsumer>();
            services.AddSingleton<UpdateUserConsumer>();
            services.AddSingleton<DeleteUserConsumer>();
            services.AddScoped<RpcClient>();
            services.AddScoped<ControllerHelper>();

            services.AddScoped<IPublisher, Publisher>();
            services.AddScoped<LogLogic>();
            services.AddSingleton(service => AutoMapperConfig.Config.CreateMapper());

            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IRefreshTokenDal, RefreshTokenDal>();
            services.AddScoped<IPendingLoginDal, PendingLoginDal>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            new List<IConsumer>
            {
                app.ApplicationServices.GetService<AddUserConsumer>(),
                app.ApplicationServices.GetService<UpdateUserConsumer>(),
                app.ApplicationServices.GetService<DeleteUserConsumer>()
            }.ForEach(consumer => consumer.Consume());


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
