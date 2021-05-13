using System.Data;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
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

            services.AddControllers();
            AddDependencies(ref services);
        }

        public void AddDependencies(ref IServiceCollection services)
        {
            services.AddScoped<ControllerHelper>();
            services.AddScoped<IPublisher, Publisher>();

            services.AddSingleton(service => new RabbitMqChannel().GetChannel());
            services.AddSingleton(service => AutoMapperConfig.Config.CreateMapper());

            services.AddScoped<UserLogic>();
            services.AddScoped<LogLogic>();
            services.AddScoped<JwtLogic>();

            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IHobbyDal, HobbyDal>();
            services.AddScoped<IArtistDal, ArtistDal>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var channel = app.ApplicationServices.GetService<IModel>();
            var userLogic = app.ApplicationServices.GetService<UserLogic>();
            var logLogic = app.ApplicationServices.GetService<LogLogic>();

            // ReSharper disable once ObjectCreationAsStatement
            new RpcServer(channel, RabbitMqQueues.FindUserQueue, userLogic.Find, logLogic);

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
