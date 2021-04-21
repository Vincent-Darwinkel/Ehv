using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Event_Service.RabbitMq
{
    public class RpcServerEventExists
    {
        public RpcServerEventExists(IModel channel)
        {
            channel.QueueDeclare("rpc_queue", false,
                false, false, null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("rpc_queue",
                false, consumer);

            consumer.Received += (model, ea) =>
            {
                string response = null;

                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    response = message;
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    channel.BasicPublish("", props.ReplyTo,
                        replyProps, responseBytes);
                    channel.BasicAck(ea.DeliveryTag,
                        false);
                }
            };
        }
    }
}