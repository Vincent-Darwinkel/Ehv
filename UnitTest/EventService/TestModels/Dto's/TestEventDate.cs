using Event_Service.Models;
using System;
using System.Collections.Generic;

namespace UnitTest.EventService.TestModels
{
    public class TestEventDate
    {
        public readonly EventDateDto Date = new EventDateDto
        {
            DateTime = DateTime.Now.AddDays(5),
            EventUuid = Guid.Parse("16ef189e-5abc-4b41-84b8-82590a693d2c"),
            Uuid = Guid.Parse("39c2033e-f348-497a-8fbc-f369fad75828"),
            EventDateUsers = new List<EventDateUserDto> { new TestEventDateUsersDto().EventDateUser },
        };
        public readonly EventDateDto DateNotLinked = new EventDateDto
        {
            DateTime = DateTime.Now.AddDays(5),
            EventUuid = Guid.Parse("0dba9bca-f927-42b3-8e74-d201c9616fc0"),
            Uuid = Guid.Parse("41a0bd7c-f276-4ed2-9392-e984cd69d86a"),
        };
    }
}