using System;

namespace Event_Service.Models.ToFrontend
{
    public class EventDateUserViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid EventDateUuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Username { get; set; }
    }
}
