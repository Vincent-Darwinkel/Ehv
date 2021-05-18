using Event_Service.Models;
using System;
using UnitTest.EventService.TestModels.Helpers;

namespace UnitTest.EventService.TestModels
{
    public class TestEventStepUserDto
    {
        public readonly EventStepUserDto EventStepUser = new EventStepUserDto
        {
            Uuid = Guid.Parse("db8ac762-adbd-40ca-b539-65676d627a6b"),
            UserUuid = new TestUser().User.Uuid,
            EventStepUuid = Guid.Parse("298bc5ad-0219-4930-8496-5642558b5d20")
        };
    }
}