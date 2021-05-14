using System;
using System.Collections.Generic;

namespace Event_Service.Models.ToFrontend
{
    public class EventDateViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid EventUuid { get; set; }
        public DateTime DateTime { get; set; }
        public bool Subscribed { get; set; } // if requesting user has subscribed to this date
        public List<EventDateUserViewmodel> EventDateUsers { get; set; } = new List<EventDateUserViewmodel>();
    }
}
