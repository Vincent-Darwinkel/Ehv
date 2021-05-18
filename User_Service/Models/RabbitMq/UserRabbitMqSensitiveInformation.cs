using System;
using User_Service.Enums;

namespace User_Service.Models.RabbitMq
{
    public class UserRabbitMqSensitiveInformation
    {
        public Guid Uuid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool ReceiveEmail { get; set; }
        public AccountRole AccountRole { get; set; }
    }
}