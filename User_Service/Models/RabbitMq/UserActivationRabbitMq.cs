using System;

namespace User_Service.Models.RabbitMq
{
    public class UserActivationRabbitMq
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Code { get; set; }
    }
}