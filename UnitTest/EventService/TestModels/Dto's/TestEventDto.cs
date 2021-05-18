using Event_Service.Models;
using System;
using System.Collections.Generic;
using UnitTest.EventService.TestModels.Helpers;

namespace UnitTest.EventService.TestModels
{
    public class TestEventDto
    {
        public readonly EventDto Event = new EventDto
        {
            Uuid = Guid.Parse("16ef189e-5abc-4b41-84b8-82590a693d2c"),
            AuthorUuid = new TestUser().User.Uuid,
            Description = "Test",
            Location = "Test location",
            Title = "Test event",
            EventDates = new List<EventDateDto> { new TestEventDate().Date },
            EventSteps = new List<EventStepDto> { new TestEventStepDto().EventStep }
        };
    }
}