using System;
using System.Collections.Generic;

namespace Event_Service.Models
{
    public class EventDateDto
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public Guid EventUuid { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; }
        public List<EventDateUserDto> EventDateUsers { get; set; } = new List<EventDateUserDto>();
    }
}