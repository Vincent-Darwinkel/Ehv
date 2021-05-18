using Event_Service.Models.RabbitMq;
using System;

namespace UnitTest.EventService.TestModels.RabbitMq
{
    public class TestEventStepRabbitMq
    {
        public readonly EventStepRabbitMq EventStepRabbitMq = new EventStepRabbitMq
        {
            Uuid = Guid.Parse("8f22a662-8bf6-4255-a75b-842016c12f34"),
            EventUuid = Guid.Parse("10d27e87-0324-4e20-bc01-52586a49c1f0"),
            StepNr = 0,
            Text = "Test step"
        };
    }
}