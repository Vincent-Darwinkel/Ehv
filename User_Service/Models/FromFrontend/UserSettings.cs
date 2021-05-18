using System;

namespace User_Service.Models.FromFrontend
{
    public class UserSettings
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public bool ReceiveEmail { get; set; }
    }
}