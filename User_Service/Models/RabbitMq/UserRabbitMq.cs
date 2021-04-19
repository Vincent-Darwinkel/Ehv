using System;
using User_Service.Enums;

namespace User_Service.Models.RabbitMq
{
    public class UserRabbitMq
    {
        public Guid Uuid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AccountRole AccountRole { get; set; }
    }
}