using System;
using System.Collections.Generic;

namespace Event_Service.Models
{
    public class EventDto
    {
        public Guid Uuid { get; set; }
        public Guid AuthorUuid { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public List<EventDateDto> EventDates { get; set; } = new List<EventDateDto>();
        public List<EventStepDto> EventSteps { get; set; } = new List<EventStepDto>();
    }
}