using System;
using User_Service.Enums;

namespace User_Service.Models
{
    public class DisabledUserDto
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public Guid UserUuid { get; set; }
        public DisableReason Reason { get; set; }
    }
}
