using Datepicker_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using System;

namespace Datepicker_Service.RabbitMq
{
    public class RabbitMqChannel
    {
        private readonly RabbitMqConfig _config;

        public RabbitMqChannel(RabbitMqConfig config)
        {
            _config = config;
        }

        public IModel GetChannel()
        {
            var rabbitMqFactory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME"),
                UserName = _config.Username,
                Password = _config.Password
            };

            IConnection connection = null;

            var attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    attempts++;
                    connection = rabbitMqFactory.CreateConnection();
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("RabbitMq connection could not be reached, attempting again in 5 seconds");
                    System.Threading.Thread.Sleep(5000);
                }
            }
            if (connection == null)
            {
                throw new ConnectionAbortedException();
            }

            return connection.CreateModel();
        }
    }
}