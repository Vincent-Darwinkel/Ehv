using System;

namespace Authentication_Service.Models.RabbitMq
{
    public class ActivationUserRabbitMq
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Code { get; set; }
    }
}