using Datepicker_Service.Logic;
using Datepicker_Service.Models.HelperFiles;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Datepicker_Service.RabbitMq.Consumers
{
    public class DeleteUserConsumer : IConsumer
    {
        private readonly IModel _channel;
        private readonly DatepickerAvailabilityLogic _datepickerAvailabilityLogic;
        private readonly DatepickerDateLogic _datepickerDateLogic;
        private readonly LogLogic _logLogic;

        public DeleteUserConsumer(IModel channel, DatepickerAvailabilityLogic datepickerAvailabilityLogic,
            DatepickerDateLogic datepickerDateLogic, LogLogic logLogic)
        {
            _channel = channel;
            _datepickerAvailabilityLogic = datepickerAvailabilityLogic;
            _datepickerDateLogic = datepickerDateLogic;
            _logLogic = logLogic;
        }

        public void Consume()
        {
            _channel.ExchangeDeclare(RabbitMqExchange.DatepickerExchange, ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqQueues.DeleteUserQueue, true, false, false, null);
            _channel.QueueBind(RabbitMqQueues.DeleteUserQueue, RabbitMqExchange.DatepickerExchange, RabbitMqRouting.RemoveUserFromDatepickerService);
            _channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                try
                {
                    byte[] body = e.Body.ToArray();
                    string json = Encoding.UTF8.GetString(body);
                    var userUuid = Newtonsoft.Json.JsonConvert.DeserializeObject<Guid>(json);

                    await _datepickerAvailabilityLogic.DeleteUserFromAvailability(userUuid);
                    await _datepickerDateLogic.DeleteUserFromDate(userUuid);
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