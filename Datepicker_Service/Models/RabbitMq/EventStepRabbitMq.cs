using System;

namespace Datepicker_Service.Models.RabbitMq
{
    public class EventStepRabbitMq
    {
        public Guid Uuid { get; set; }
        public int StepNr { get; set; }
        public Guid EventUuid { get; set; }
        public string Text { get; set; }
    }
}