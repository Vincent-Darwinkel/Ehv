using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace User_Service.RabbitMq.Rpc
{
    public class RpcClient : IRpcClient
    {
        private readonly IModel _channel;
        private string _replyQueueName;
        private EventingBasicConsumer _consumer;
        private readonly BlockingCollection<string> _respQueue = new BlockingCollection<string>();
        private IBasicProperties _props;

        public RpcClient(IModel channel)
        {
            _channel = channel;
        }

        private void Configure()
        {
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

        public T Call<T>(object objectToSend, string queue)
        {
            if (objectToSend == null || string.IsNullOrEmpty(queue))
            {
                throw new NullReferenceException();
            }

            if (string.IsNullOrEmpty(_replyQueueName))
            {
                Configure();
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectToSend);
            var messageBytes = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(
                "",
                queue, // this parameter name is routing key but needs the name of the queue, the name is probably wrong
                _props,
                messageBytes);

            _channel.BasicConsume(
                consumer: _consumer,
                queue: _replyQueueName,
                autoAck: true);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(_respQueue.Take());
        }
    }
}