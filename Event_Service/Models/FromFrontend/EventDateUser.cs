using System;

namespace Event_Service.Models.FromFrontend
{
    public class EventDateUser
    {
        public Guid Uuid { get; set; }
        public Guid EventDateUuid { get; set; }
        public Guid UserUuid { get; set; }
    }
}