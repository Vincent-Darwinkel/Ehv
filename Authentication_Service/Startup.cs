using System.Linq;
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

namespace Authentication_Service
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
            services.AddDbContextPool<DataContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(ServerVersion.AutoDetect(connectionString)));

            services.AddControllers();
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            AddDependencyInjection(ref services);
        }

        private void AddDependencyInjection(ref IServiceCollection services)
        {
            services.AddScoped<UserLogic>();
            services.AddSingleton(service => new RabbitMqChannel().GetChannel());
            services.AddSingleton<AddUserConsumer>();
            services.AddSingleton<UpdateUserConsumer>();
            services.AddSingleton<DeleteUserConsumer>();

            services.AddScoped<IPublisher, Publisher>();
            services.AddScoped<LogLogic>();
            services.AddSingleton(service => AutoMapperConfig.Config.CreateMapper());

            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IRefreshTokenDal, RefreshTokenDal>();
            services.AddScoped<IPasswordResetDal, PasswordResetDal>();
            services.AddScoped<IDisabledUserDal, DisabledUserDal>();
            services.AddScoped<IActivationDal, ActivationDal>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AddUserConsumer consumer = app.ApplicationServices.GetService<AddUserConsumer>();
            consumer.Consume();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
