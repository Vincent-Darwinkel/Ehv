using System;
using System.Collections.Generic;

namespace Event_Service.Models.FromFrontend
{
    public class EventDate
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public Guid EventUuid { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; }
        public List<EventDateUser> EventDateUsers { get; set; } = new List<EventDateUser>();
    }
}