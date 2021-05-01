using System;
using System.Text;
using Logging_Service.Logic;
using Logging_Service.Models.Helpers;
using Logging_Service.Models.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Logging_Service.RabbitMq.Consumers
{
    public class AddLogConsumer : IConsumer
    {
        private readonly IModel _channel;
        private readonly LogLogic _logLogic;

        public AddLogConsumer(IServiceProvider serviceProvider, IModel channel)
        {
            _channel = channel;
            using var scope = serviceProvider.CreateScope();
            _logLogic = scope.ServiceProvider.GetRequiredService<LogLogic>();
        }

        /// <summary>
        /// This method listens for email messages on the message queue and sends an email if it receives a message
        /// </summary>
        public void Consume()
        {
            _channel.ExchangeDeclare(RabbitMqExchange.LogExchange, ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqQueues.LoggingQueue, true, false, false, null);
            _channel.QueueBind(RabbitMqQueues.LoggingQueue, RabbitMqExchange.LogExchange, RabbitMqRouting.AddLog);
            _channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                try
                {
                    byte[] body = e.Body.ToArray();
                    string json = Encoding.UTF8.GetString(body);
                    var log = Newtonsoft.Json.JsonConvert.DeserializeObject<LogRabbitMq>(json);

                    await _logLogic.Add(log);
                }
                catch (Exception exception)
                {
                    await _logLogic.Log(exception);
                }
            };

            _channel.BasicConsume(RabbitMqQueues.LoggingQueue, true, consumer);
        }
    }
}