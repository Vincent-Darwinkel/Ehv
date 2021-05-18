using System;
using System.Collections.Generic;

namespace Event_Service.Models.ToFrontend
{
    public class EventViewmodel
    {
        public Guid Uuid { get; set; }
        public bool CanBeRemoved { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public List<EventDateViewmodel> EventDates { get; set; } = new List<EventDateViewmodel>();
        public List<EventStepViewmodel> EventSteps { get; set; } = new List<EventStepViewmodel>();
    }
}
