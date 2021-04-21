using System;
using System.Collections.Generic;

namespace Event_Service.Models.FromFrontend
{
    public class EventStep
    {
        public Guid Uuid { get; set; }
        public int StepNr { get; set; }
        public Guid EventUuid { get; set; }
        public string Description { get; set; }
        public List<EventStepUser> EventStepUsers { get; set; } = new List<EventStepUser>();
    }
}