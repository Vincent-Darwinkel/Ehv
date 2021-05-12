using System.Linq;
using Favorite_Artist_Service.Dal;
using Favorite_Artist_Service.Dal.Interfaces;
using Favorite_Artist_Service.Logic;
using Favorite_Artist_Service.Model.Helpers;
using Favorite_Artist_Service.RabbitMq.Rpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Favorite_Artist_Service
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
            services.AddControllers();
        }

        public void AddDependencies(ref IServiceCollection services)
        {
            services.AddSingleton(service => AutoMapperConfig.Config.CreateMapper());
            services.AddScoped<LogLogic>();
            services.AddScoped<IFavoriteArtistDal, FavoriteArtistDal>();
            services.AddScoped<FavoriteArtistLogic>();
            services.AddScoped<JwtLogic>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var channel = app.ApplicationServices.GetService<IModel>();
            var favoriteArtistLogic = app.ApplicationServices.GetService<FavoriteArtistLogic>();
            var logLogic = app.ApplicationServices.GetService<LogLogic>();

            // ReSharper disable once ObjectCreationAsStatement
            new RpcServer(channel, RabbitMqQueues.AllArtistQueue, favoriteArtistLogic.AllRabbitMq, logLogic);

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
