using RabbitMQ.Client;

namespace Logging_Service.RabbitMq
{
    public class RabbitMqChannel
    {
        public IModel GetChannel()
        {
            var rabbitMqFactory = new ConnectionFactory { HostName = "rabbitmq", UserName = "guest", Password = "guest" };
            var connection = rabbitMqFactory.CreateConnection();
            return connection.CreateModel();
        }
    }
}