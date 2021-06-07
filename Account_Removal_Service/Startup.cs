using Account_Removal_Service.Logic;
using Account_Removal_Service.Models.Helpers;
using Account_Removal_Service.RabbitMq;
using Account_Removal_Service.RabbitMq.Publishers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Account_Removal_Service
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
            AddDependencies(ref services);
            services.AddControllers();
        }

        public void AddDependencies(ref IServiceCollection services)
        {
            IConfigurationSection rabbitMqSection = _config.GetSection(nameof(RabbitMqConfig));
            services.AddScoped(service => new RabbitMqChannel(rabbitMqSection.Get<RabbitMqConfig>()).GetChannel());
            services.AddScoped<IPublisher, Publisher>();
            services.AddScoped<AccountRemovalLogic>();
            services.AddScoped<LogLogic>();
            services.AddScoped<ControllerHelper>();
            services.AddScoped<JwtLogic>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
