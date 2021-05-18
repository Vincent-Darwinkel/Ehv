using Event_Service.Models;
using System;
using System.Collections.Generic;

namespace UnitTest.EventService.TestModels
{
    public class TestEventStepDto
    {
        public readonly EventStepDto EventStep = new EventStepDto
        {
            Uuid = Guid.Parse("298bc5ad-0219-4930-8496-5642558b5d20"),
            EventStepUsers = new List<EventStepUserDto> { new TestEventStepUserDto().EventStepUser },
            EventUuid = Guid.Parse("16ef189e-5abc-4b41-84b8-82590a693d2c"),
            StepNr = 0,
            Text = "Test step 1"
        };
        public readonly EventStepDto EventStepNoUsers = new EventStepDto
        {
            Uuid = Guid.Parse("6595fddb-74d9-4d48-ab0c-26c60e40a42f"),
            EventUuid = Guid.Parse("16ef189e-5abc-4b41-84b8-82590a693d2c"),
            StepNr = 0,
            Text = "Test step 1"
        };
    }
}