using System;
using User_Service.Enums;

namespace User_Service.Models.FromFrontend
{
    public class DisabledUser
    {
        public Guid UserUuid { get; set; }
        public DisableReason Reason { get; set; }
    }
}