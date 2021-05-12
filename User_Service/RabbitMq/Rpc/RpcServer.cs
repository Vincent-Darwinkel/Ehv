using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using User_Service.Logic;

namespace User_Service.RabbitMq.Rpc
{
    public class RpcServer
    {
        public RpcServer(IModel channel, string queue, Func<string, Task<string>> callbackMethod, LogLogic logLogic)
        {
            channel.QueueDeclare(queue, false, false, false, null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue,
                false, consumer);

            consumer.Received += async (model, ea) =>
            {
                string response = null;

                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    string message = Encoding.UTF8.GetString(body);
                    response = await callbackMethod(message);
                }
                catch (Exception e)
                {
                    logLogic.Log(e);
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
        }
    }
}
