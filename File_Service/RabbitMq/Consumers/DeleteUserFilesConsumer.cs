using File_Service.Logic;
using File_Service.Models.HelperFiles;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace File_Service.RabbitMq.Consumers
{
    public class DeleteUserFilesConsumer : IConsumer
    {
        private readonly IModel _channel;
        private readonly LogLogic _logLogic;

        public DeleteUserFilesConsumer(IModel channel, LogLogic logLogic)
        {
            _channel = channel;
            _logLogic = logLogic;
        }

        /// <summary>
        /// This method listens for user file removal messages on the message and removes all files owned by the user
        /// </summary>
        public void Consume()
        {
            _channel.ExchangeDeclare(RabbitMqExchange.FileExchange, ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqQueues.DeleteUserQueue, true, false, false, null);
            _channel.QueueBind(RabbitMqQueues.DeleteUserQueue, RabbitMqExchange.FileExchange, RabbitMqRouting.DeleteUser);
            _channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                try
                {
                    byte[] body = e.Body.ToArray();
                    //todo add files
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