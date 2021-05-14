using System;
using Authentication_Service.Enums;

namespace Authentication_Service.Models.RabbitMq
{
    public class DisabledUserRabbitMq
    {
        public Guid Uuid { get; set; }
        public DisableReason Reason { get; set; }
        public Guid UserUuid { get; set; }
    }
}