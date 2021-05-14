using System;
using Event_Service.Enums;

namespace Event_Service.Models.HelperFiles
{
    public class UserHelper
    {
        public Guid Uuid { get; set; }
        public AccountRole AccountRole { get; set; }
    }
}
