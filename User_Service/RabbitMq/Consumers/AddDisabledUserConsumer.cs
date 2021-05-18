using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using User_Service.Logic;
using User_Service.Models.HelperFiles;
using User_Service.Models.RabbitMq;

namespace User_Service.RabbitMq.Consumers
{
    public class AddDisabledUserConsumer : IConsumer
    {
        private readonly IModel _channel;
        private readonly DisabledUserLogic _disabledUserLogic;
        private readonly LogLogic _logLogic;

        public AddDisabledUserConsumer(IModel channel, DisabledUserLogic disabledUserLogic, LogLogic logLogic)
        {
            _channel = channel;
            _disabledUserLogic = disabledUserLogic;
            _logLogic = logLogic;
        }

        /// <summary>
        /// This method listens for email messages on the message queue and sends an email if it receives a message
        /// </summary>
        public void Consume()
        {
            _channel.ExchangeDeclare(RabbitMqExchange.UserExchange, ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqQueues.AddActivationQueue, true, false, false, null);
            _channel.QueueBind(RabbitMqQueues.AddActivationQueue, RabbitMqExchange.UserExchange, RabbitMqRouting.AddActivation);
            _channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                try
                {
                    byte[] body = e.Body.ToArray();
                    string json = Encoding.UTF8.GetString(body);
                    var disabledUserRabbitMq = Newtonsoft.Json.JsonConvert.DeserializeObject<DisabledUserRabbitMq>(json);

                    await _disabledUserLogic.Add(disabledUserRabbitMq);
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