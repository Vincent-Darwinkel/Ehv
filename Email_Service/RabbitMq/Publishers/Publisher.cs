using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Email_Service.Logic;

namespace Email_Service.RabbitMq.Publishers
{
    public class Publisher : IPublisher
    {
        private readonly IModel _channel;

        public Publisher(IModel channel)
        {
            _channel = channel;
        }

        public void Publish(object objectToSend, string routingKey, string exchange)
        {
            try
            {
                Send(objectToSend, routingKey, exchange);
            }
            catch (AlreadyClosedException)
            {
                QueuedTasks.QueueMethod(() => Send(objectToSend, routingKey, exchange));
            }
        }

        private void Send(object objectToSend, string routingKey, string exchange)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000}
            };

            _channel.ExchangeDeclare(exchange, ExchangeType.Direct, arguments: ttl);
            string message = Newtonsoft.Json.JsonConvert.SerializeObject(objectToSend);
            byte[] body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange,
                routingKey,
                null,
                body);
        }
    }
}