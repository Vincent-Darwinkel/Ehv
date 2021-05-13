using System;
using Authentication_Service.Enums;

namespace Authentication_Service.Models.Dto
{
    public class DisabledUserDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public DisableReason Reason { get; set; }
    }
}
