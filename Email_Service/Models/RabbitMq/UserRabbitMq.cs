using System;

namespace Email_Service.Models.RabbitMq
{
    public class UserRabbitMq
    {
        public Guid Uuid { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}