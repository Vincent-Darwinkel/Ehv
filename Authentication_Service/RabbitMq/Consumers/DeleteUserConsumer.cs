using Authentication_Service.Logic;
using Authentication_Service.Models.HelperFiles;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Authentication_Service.RabbitMq.Consumers
{
    public class DeleteUserConsumer : IConsumer
    {
        private readonly IModel _channel;
        private readonly UserLogic _userLogic;
        private readonly LogLogic _logLogic;

        public DeleteUserConsumer(IModel channel, UserLogic userLogic, LogLogic logLogic)
        {
            _channel = channel;
            _userLogic = userLogic;
            _logLogic = logLogic;
        }

        public void Consume()
        {
            _channel.ExchangeDeclare(RabbitMqExchange.AuthenticationExchange, ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqQueues.DeleteUserQueue, true, false, false, null);
            _channel.QueueBind(RabbitMqQueues.DeleteUserQueue, RabbitMqExchange.AuthenticationExchange, RabbitMqRouting.DeleteUser);
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
