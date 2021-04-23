using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using User_Service.Logic;
using User_Service.Models;
using User_Service.Models.HelperFiles;
using User_Service.Models.ToFrontend;

namespace User_Service.RabbitMq
{
    public class RpcServerGetByUuid
    {
        private readonly UserLogic _userLogic;
        private readonly IMapper _mapper;

        public RpcServerGetByUuid(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            _userLogic = scope.ServiceProvider.GetRequiredService<UserLogic>(); // TODO ask teacher if this is a good solution
            _mapper = scope.ServiceProvider.GetRequiredService<IMapper>(); // TODO ask teacher if this is a good solution
        }

        /// <summary>
        /// Waits for an client to send a request to get all the users that matches the uuid collection in the message
        /// </summary>
        /// <param name="channel"></param>
        public RpcServerGetByUuid(IModel channel)
        {
            channel.QueueDeclare(RabbitMqQueues.FindUserQueue, false,
                false, false, null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(RabbitMqQueues.FindUserQueue,
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
                    string message = Encoding.UTF8.GetString(body);
                    var uuidCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Guid>>(message);

                    List<UserDto> foundUsers = _userLogic.Find(uuidCollection).Result;
                    var userViewModels = _mapper.Map<List<UserViewModel>>(foundUsers);
                    response = Newtonsoft.Json.JsonConvert.SerializeObject(userViewModels);
                }
                catch (Exception e)
                {
                    throw e; // TODO add logging
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