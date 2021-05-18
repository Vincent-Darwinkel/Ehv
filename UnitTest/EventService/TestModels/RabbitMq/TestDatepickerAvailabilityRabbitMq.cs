using Event_Service.Models.RabbitMq;
using System;
using UnitTest.EventService.TestModels.Helpers;

namespace UnitTest.EventService.TestModels.RabbitMq
{
    public class TestDatepickerAvailabilityRabbitMq
    {
        public DatepickerAvailabilityRabbitMq DatepickerAvailabilityRabbitMq = new DatepickerAvailabilityRabbitMq
        {
            Uuid = Guid.Parse("e8fc3c24-87a2-46fe-b7b6-b2494d369a72"),
            DateUuid = Guid.Parse("7acad80c-5b8e-4d2d-aa0d-28e467662538"),
            UserUuid = new TestUser().User.Uuid
        };
    }
}