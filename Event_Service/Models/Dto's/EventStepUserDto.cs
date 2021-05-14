using System;

namespace Event_Service.Models
{
    public class EventStepUserDto
    {
        public Guid Uuid { get; set; }
        public Guid EventStepUuid { get; set; }
        public Guid UserUuid { get; set; }
    }
}