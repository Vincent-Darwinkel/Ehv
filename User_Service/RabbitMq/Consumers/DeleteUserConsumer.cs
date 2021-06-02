using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using User_Service.Logic;
using User_Service.Models.HelperFiles;

namespace User_Service.RabbitMq.Consumers
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

        /// <summary>
        /// This method listens for email messages on the message queue and sends an email if it receives a message
        /// </summary>
        public void Consume()
        {
            _channel.ExchangeDeclare(RabbitMqExchange.UserExchange, ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqQueues.AddActivationQueue, true, false, false, null);
            _channel.QueueBind(RabbitMqQueues.DeleteUserQueue, RabbitMqExchange.UserExchange, RabbitMqRouting.DeleteUser);
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

            _channel.BasicConsume(RabbitMqQueues.AddActivationQueue, true, consumer);
        }
    }
}