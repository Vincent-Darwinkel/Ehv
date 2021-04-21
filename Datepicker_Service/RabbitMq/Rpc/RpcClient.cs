using System;
using System.Collections.Concurrent;
using System.Text;
using Datepicker_Service.Models.HelperFiles;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Datepicker_Service.RabbitMq.Rpc
{
    public class RpcClient
    {
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;
        private readonly BlockingCollection<string> _respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties _props;

        public RpcClient(IModel channel)
        {
            _replyQueueName = channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(channel);

            _props = channel.CreateBasicProperties();
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

            _channel = channel;
        }

        public T Call<T>(object objectToSend, string routingKey)
        {
            try
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectToSend);
                var messageBytes = Encoding.UTF8.GetBytes(json);
                _channel.BasicPublish(
                    "",
                    routingKey,
                    _props,
                    messageBytes);

                _channel.BasicConsume(
                    consumer: _consumer,
                    queue: _replyQueueName,
                    autoAck: true);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(_respQueue.Take());
            }
            finally
            {
                _channel.Close();
            }
        }
    }
}