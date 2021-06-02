using Event_Service.Logic;
using Event_Service.Models.HelperFiles;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Event_Service.RabbitMq.Consumers
{
    public class DeleteUserConsumer : IConsumer
    {
        private readonly IModel _channel;
        private readonly EventDateUserLogic _eventDateUserLogic;
        private readonly EventStepUserLogic _eventStepUserLogic;
        private readonly LogLogic _logLogic;

        public DeleteUserConsumer(IModel channel, EventDateUserLogic eventDateUserLogic,
            EventStepUserLogic eventStepUserLogic, LogLogic logLogic)
        {
            _channel = channel;
            _eventDateUserLogic = eventDateUserLogic;
            _eventStepUserLogic = eventStepUserLogic;
            _logLogic = logLogic;
        }

        /// <summary>
        /// This method listens for email messages on the message queue and sends an email if it receives a message
        /// </summary>
        public void Consume()
        {
            _channel.ExchangeDeclare(RabbitMqExchange.EventExchange, ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqQueues.DeleteUserQueue, true, false, false, null);
            _channel.QueueBind(RabbitMqQueues.DeleteUserQueue, RabbitMqExchange.EventExchange, RabbitMqRouting.DeleteUser);
            _channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                try
                {
                    byte[] body = e.Body.ToArray();
                    string json = Encoding.UTF8.GetString(body);
                    var userUuid = Newtonsoft.Json.JsonConvert.DeserializeObject<Guid>(json);

                    await _eventDateUserLogic.DeleteUserFromEventDate(userUuid);
                    await _eventStepUserLogic.DeleteUserFromSteps(userUuid);
                }
                catch (Exception exception)
                {
                    _logLogic.Log(exception);
                }
            };

            _channel.BasicConsume(RabbitMqQueues.ConvertDatepickerQueue, true, consumer);
        }
    }
}