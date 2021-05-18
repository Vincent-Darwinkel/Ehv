using Event_Service.Models.RabbitMq;
using System;
using System.Collections.Generic;
using UnitTest.EventService.TestModels.Helpers;

namespace UnitTest.EventService.TestModels.RabbitMq
{
    public class TestDatepickerRabbitMq
    {
        public readonly DatepickerRabbitMq DatepickerRabbit = new DatepickerRabbitMq
        {
            Uuid = Guid.Parse("10d27e87-0324-4e20-bc01-52586a49c1f0"),
            AuthorUuid = new TestUser().User.Uuid,
            Title = "Test",
            Description = "Test description",
            Dates = new List<DatepickerDateRabbitMq> { new TestDatepickerDateRabbitMq().DatepickerDateRabbit },
            EventSteps = new List<EventStepRabbitMq> { new TestEventStepRabbitMq().EventStepRabbitMq },
            Expires = DateTime.Now.AddDays(5),
            Location = "Test location",
            SelectedDates = new List<Guid> { new TestDatepickerDateRabbitMq().DatepickerDateRabbit.Uuid }
        };
    }
}