using System;
using Authentication_Service.Enums;

namespace Authentication_Service.Models.RabbitMq
{
    public class UserRabbitMqSensitiveInformation
    {
        public Guid Uuid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public AccountRole AccountRole { get; set; }
    }
}