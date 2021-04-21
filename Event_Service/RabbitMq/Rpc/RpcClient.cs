using System;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json.Serialization;
using Event_Service.Models.HelperFiles;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Event_Service.RabbitMq.RPC
{
    public class RpcClient
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;
        private readonly BlockingCollection<string> _respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties _props;

        public RpcClient()
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _replyQueueName = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);

            _props = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            _props.CorrelationId = correlationId;
            _props.ReplyTo = _replyQueueName;

            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    _respQueue.Add(response);
                }
            };
        }

        public string Call(object objectToSend)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectToSend);
            var messageBytes = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(
                "",
                RabbitMqRouting.EventExists,
                _props,
                messageBytes);

            _channel.BasicConsume(
                consumer: _consumer,
                queue: _replyQueueName,
                autoAck: true);

            return _respQueue.Take();
        }

        public void Close()
        {
            _connection.Close();
        }
    }
}
