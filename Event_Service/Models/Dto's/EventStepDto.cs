using System;
using System.Collections.Generic;

namespace Event_Service.Models
{
    public class EventStepDto
    {
        public Guid Uuid { get; set; }
        public int StepNr { get; set; }
        public Guid EventUuid { get; set; }
        public string Text { get; set; }
        public List<EventStepUserDto> EventStepUsers { get; set; } = new List<EventStepUserDto>();
    }
}