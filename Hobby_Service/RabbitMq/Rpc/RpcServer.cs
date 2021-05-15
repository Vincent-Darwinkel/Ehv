using Hobby_Service.Logic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Hobby_Service.RabbitMq.Rpc
{
    public class RpcServer
    {
        private readonly LogLogic _logLogic;

        public RpcServer(IModel channel, string queue, Func<Task<string>> callbackMethod, LogLogic logLogic)
        {
            _logLogic = logLogic;

            channel.QueueDeclare(queue, false, false, false, null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue,
                false, consumer);

            Configure(channel, consumer, callbackMethod);
        }

        public RpcServer(IModel channel, string queue, Func<string, Task<string>> callbackMethod, LogLogic logLogic)
        {
            _logLogic = logLogic;

            channel.QueueDeclare(queue, false, false, false, null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue,
                false, consumer);

            Configure(channel, consumer, callbackMethod);
        }

        private void Configure(IModel channel, EventingBasicConsumer consumer, Func<string, Task<string>> callbackMethod)
        {
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
                    _logLogic.Log(e);
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
        }

        private void Configure(IModel channel, EventingBasicConsumer consumer, Func<Task<string>> callbackMethod)
        {
            consumer.Received += async (model, ea) =>
            {
                string response = null;

                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    response = await callbackMethod();
                }
                catch (Exception e)
                {
                    _logLogic.Log(e);
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