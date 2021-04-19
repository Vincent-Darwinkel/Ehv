using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace User_Service.RabbitMq.Publishers
{
    public class UserProducer
    {
        private readonly IModel _channel;

        public UserProducer(IModel channel)
        {
            _channel = channel;
        }

        public void Publish(string message, string routingKey)
        {
            var ttl = new Dictionary<string, object>
                {
                    {"x-message-ttl", 30000}
                };

            _channel.ExchangeDeclare("user_exchange", ExchangeType.Direct, arguments: ttl);

            byte[] body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish("user_exchange",
                routingKey,
                null,
                body);
        }
    }
}