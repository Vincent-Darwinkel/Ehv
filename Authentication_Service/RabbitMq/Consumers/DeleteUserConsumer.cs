using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication_Service.Logic;
using Authentication_Service.Models.HelperFiles;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Authentication_Service.RabbitMq.Consumers
{
    public class DeleteUserConsumer : IConsumer
    {
        private readonly IModel _channel;
        private readonly UserLogic _userLogic;
        private readonly LogLogic _logLogic;

        public DeleteUserConsumer(IServiceProvider serviceProvider, IModel channel)
        {
            _channel = channel;
            using var scope = serviceProvider.CreateScope();
            _userLogic = scope.ServiceProvider.GetRequiredService<UserLogic>();
            _logLogic = scope.ServiceProvider.GetRequiredService<LogLogic>();
        }

        public void Consume()
        {
            _channel.ExchangeDeclare("user_exchange", ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqQueues.DeleteUserQueue, true, false, false, null);
            _channel.QueueBind(RabbitMqQueues.DeleteUserQueue, "user_exchange", RabbitMqRouting.DeleteUser);
            _channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                try
                {
                    byte[] body = e.Body.ToArray();
                    string json = Encoding.UTF8.GetString(body);
                    var userUuid = Newtonsoft.Json.JsonConvert.DeserializeObject<Guid>(json);

                    await _userLogic.Delete(userUuid);
                }
                catch (Exception exception)
                {
                    _logLogic.Log(exception);
                }
            };

            _channel.BasicConsume(RabbitMqQueues.DeleteUserQueue, true, consumer);
        }
    }
}
