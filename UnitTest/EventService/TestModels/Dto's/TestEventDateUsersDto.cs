using Event_Service.Models;
using System;
using UnitTest.EventService.TestModels.Helpers;

namespace UnitTest.EventService.TestModels
{
    public class TestEventDateUsersDto
    {
        public readonly EventDateUserDto EventDateUser = new EventDateUserDto
        {
            EventDateUuid = Guid.Parse("39c2033e-f348-497a-8fbc-f369fad75828"),
            UserUuid = new TestUser().User.Uuid,
            Uuid = Guid.Parse("b4eac80f-f34f-47a0-8fa0-57bdc09f8b58")
        };
    }
}