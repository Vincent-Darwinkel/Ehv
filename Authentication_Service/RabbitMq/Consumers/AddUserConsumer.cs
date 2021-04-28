﻿using System;
using System.Text;
using Authentication_Service.Logic;
using Authentication_Service.Models.Dto;
using Authentication_Service.Models.HelperFiles;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Authentication_Service.RabbitMq.Consumers
{
    public class AddUserConsumer : IConsumer
    {
        private readonly IModel _channel;
        private readonly UserLogic _userLogic;

        public AddUserConsumer(IServiceProvider serviceProvider, IModel channel)
        {
            _channel = channel;
            using var scope = serviceProvider.CreateScope();
            _userLogic = scope.ServiceProvider.GetRequiredService<UserLogic>(); // TODO ask teacher if this is a good solution
        }

        public void Consume()
        {
            _channel.ExchangeDeclare("user_exchange", ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqQueues.AddUserQueue, true, false, false, null);
            _channel.QueueBind(RabbitMqQueues.AddUserQueue, "user_exchange", RabbitMqRouting.AddUser);
            _channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                try
                {
                    byte[] body = e.Body.ToArray();
                    string json = Encoding.UTF8.GetString(body);
                    var user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDto>(json);

                    await _userLogic.Add(user);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            };

            _channel.BasicConsume(RabbitMqQueues.AddUserQueue, true, consumer);
        }
    }
}