using Authentication_Service.Logic;
using Authentication_Service.Models.HelperFiles;
using Authentication_Service.Models.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Authentication_Service.RabbitMq.Consumers
{
    public class AddUserConsumer : IConsumer
    {
        private readonly IModel _channel;
        private readonly UserLogic _userLogic;
        private readonly LogLogic _logLogic;

        public AddUserConsumer(IModel channel, UserLogic userLogic, LogLogic logLogic)
        {
            _channel = channel;
            _userLogic = userLogic;
            _logLogic = logLogic;
        }

        public void Consume()
        {
            _channel.ExchangeDeclare(RabbitMqExchange.UserExchange, ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqQueues.AddUserQueue, true, false, false, null);
            _channel.QueueBind(RabbitMqQueues.AddUserQueue, "user_exchange", RabbitMqRouting.AddUser);
            _channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                try
                {
                    byte[] body = e.Body.ToArray();
                    string json = Encoding.UTF8.GetString(body);
                    var user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserRabbitMqSensitiveInformation>(json);

                    await _userLogic.Add(user);
                }
                catch (Exception exception)
                {
                    _logLogic.Log(exception);
                }
            };

            _channel.BasicConsume(RabbitMqQueues.AddUserQueue, true, consumer);
        }
    }
}
