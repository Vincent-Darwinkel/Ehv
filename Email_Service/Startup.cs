using Email_Service.Logic;
using Email_Service.Models.Helpers;
using Email_Service.RabbitMq;
using Email_Service.RabbitMq.Consumers;
using Email_Service.RabbitMq.Publishers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Email_Service
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            AddDependencies(ref services);
        }

        public void AddDependencies(ref IServiceCollection services)
        {
            IConfigurationSection section = _config.GetSection(nameof(EmailConfig));

            services.AddSingleton(section.Get<RabbitMqConfig>());
            services.AddSingleton(section.Get<EmailConfig>());
            services.AddScoped<IPublisher, Publisher>();
            services.AddSingleton<SendMailConsumer>();
            services.AddSingleton(service => new RabbitMqChannel().GetChannel());
            services.AddScoped<LogLogic>();
            services.AddScoped<EmailLogic>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            new List<IConsumer>
            {
                app.ApplicationServices.GetService<SendMailConsumer>(),
            }.ForEach(consumer => consumer.Consume());

            app.UseRouting();
        }
    }
}
