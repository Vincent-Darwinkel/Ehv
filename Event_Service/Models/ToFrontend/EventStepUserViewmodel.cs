using System;

namespace Event_Service.Models.ToFrontend
{
    public class EventStepUserViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid EventStepUuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Username { get; set; }
    }
}
