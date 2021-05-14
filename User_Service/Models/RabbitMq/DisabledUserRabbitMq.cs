using System;
using User_Service.Enums;

namespace User_Service.Models.RabbitMq
{
    public class DisabledUserRabbitMq
    {
        public Guid Uuid { get; set; }
        public DisableReason Reason { get; set; }
        public Guid UserUuid { get; set; }
    }
}