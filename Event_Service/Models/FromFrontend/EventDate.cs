using System;
using System.Collections.Generic;

namespace Event_Service.Models.FromFrontend
{
    public class EventDate
    {
        public Guid Uuid { get; set; }
        public Guid EventUuid { get; set; }
        public DateTime DateTime { get; set; }
        public List<EventDateUser> EventDateUsers { get; set; } = new List<EventDateUser>();
    }
}