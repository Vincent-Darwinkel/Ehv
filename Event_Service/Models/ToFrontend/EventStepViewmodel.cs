using System;
using System.Collections.Generic;

namespace Event_Service.Models.ToFrontend
{
    public class EventStepViewmodel
    {
        public Guid Uuid { get; set; }
        public int StepNr { get; set; }
        public Guid EventUuid { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; } // if requesting user has completed to this step
        public List<EventStepUserViewmodel> EventStepUsers { get; set; } = new List<EventStepUserViewmodel>();
    }
}
