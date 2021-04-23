using System;
using System.Collections.Generic;

namespace Event_Service.Models.FromFrontend
{
    public class Event
    {
        public Guid Uuid { get; set; }
        public Guid AuthorUuid { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public List<EventDate> EventDates { get; set; } = new List<EventDate>();
        public List<EventStep> EventSteps { get; set; } = new List<EventStep>();
    }
}