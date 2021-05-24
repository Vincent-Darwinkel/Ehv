using Event_Service.Logic;
using Event_Service.Models.HelperFiles;
using Event_Service.Models.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Event_Service.RabbitMq.Consumers
{
    public class ConvertToEventConsumer : IConsumer
    {
        private readonly IModel _channel;
        private readonly EventLogic _eventLogic;
        private readonly LogLogic _logLogic;

        public ConvertToEventConsumer(IModel channel, EventLogic eventLogic, LogLogic logLogic)
        {
            _channel = channel;
            _eventLogic = eventLogic;
            _logLogic = logLogic;
        }

        /// <summary>
        /// This method listens for email messages on the message queue and sends an email if it receives a message
        /// </summary>
        public void Consume()
        {
            _channel.ExchangeDeclare(RabbitMqExchange.EventExchange, ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqQueues.ConvertDatepickerQueue, true, false, false, null);
            _channel.QueueBind(RabbitMqQueues.ConvertDatepickerQueue, RabbitMqExchange.EventExchange, RabbitMqRouting.ConvertDatepicker);
            _channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                try
                {
                    byte[] body = e.Body.ToArray();
                    string json = Encoding.UTF8.GetString(body);
                    var datepickerRabbitMq = Newtonsoft.Json.JsonConvert.DeserializeObject<DatepickerRabbitMq>(json);

                    await _eventLogic.ConvertToEventAsync(datepickerRabbitMq);
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