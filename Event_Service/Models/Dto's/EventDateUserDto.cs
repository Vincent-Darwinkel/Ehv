using System;

namespace Event_Service.Models
{
    public class EventDateUserDto
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public Guid EventDateUuid { get; set; }
        public Guid UserUuid { get; set; }
    }
}