using System;

namespace Authentication_Service.Models.RabbitMq
{
    public class UserRabbitMq
    {
        public Guid Uuid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}