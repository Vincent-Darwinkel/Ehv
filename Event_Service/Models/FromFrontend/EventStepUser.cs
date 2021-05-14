using System;

namespace Event_Service.Models.FromFrontend
{
    public class EventStepUser
    {
        public Guid Uuid { get; set; }
        public Guid EventStepUuid { get; set; }
        public Guid UserUuid { get; set; }
    }
}