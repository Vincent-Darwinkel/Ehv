using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using User_Service.Dal;
using User_Service.Dal.Interfaces;
using User_Service.Logic;
using User_Service.Models;
using User_Service.Models.FromFrontend;
using User_Service.Models.RabbitMq;
using User_Service.RabbitMq;
using User_Service.RabbitMq.Publishers;

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
            services.AddDbContext<DataContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Transient);

            services.AddControllers();
            AddDependencies(ref services);
        }

        public void AddDependencies(ref IServiceCollection services)
        {
            services.AddScoped<ControllerHelper>();

            services.AddSingleton(service => new RabbitMqChannel().GetChannel());
            services.AddSingleton<UserProducer>();

            services.AddSingleton(service => AutoMapperConfiguration());

            services.AddScoped<UserLogic>();
            services.AddScoped<JwtLogic>();

            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IHobbyDal, HobbyDal>();
            services.AddScoped<IArtistDal, ArtistDal>();
        }

        private IMapper AutoMapperConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<User, UserRabbitMq>();
            });

            return config.CreateMapper();
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
