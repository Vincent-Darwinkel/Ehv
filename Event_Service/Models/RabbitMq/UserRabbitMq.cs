using System;
using Event_Service.Enums;

namespace Event_Service.Models.RabbitMq
{
    public class UserRabbitMq
    {
        public Guid Uuid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public AccountRole AccountRole { get; set; }
    }
}