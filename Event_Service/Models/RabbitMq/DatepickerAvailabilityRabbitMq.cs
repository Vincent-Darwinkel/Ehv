using System;

namespace Event_Service.Models.RabbitMq
{
    public class DatepickerAvailabilityRabbitMq
    {
        public Guid Uuid { get; set; }
        public Guid DateUuid { get; set; }
        public Guid UserUuid { get; set; }
    }
}