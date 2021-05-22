using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using System;

namespace Hobby_Service.RabbitMq
{
    public class RabbitMqChannel
    {
        public IModel GetChannel()
        {
            var rabbitMqFactory = new ConnectionFactory { HostName = "rabbitmq", UserName = "guest", Password = "guest" };
            IConnection connection = null;

            var attempts = 0;
            while (attempts < 100)
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