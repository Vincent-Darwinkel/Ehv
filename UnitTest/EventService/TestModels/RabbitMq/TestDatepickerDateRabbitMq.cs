using Event_Service.Models.RabbitMq;
using System;

namespace UnitTest.EventService.TestModels.RabbitMq
{
    public class TestDatepickerDateRabbitMq
    {
        public readonly DatepickerDateRabbitMq DatepickerDateRabbit = new DatepickerDateRabbitMq
        {
            Uuid = Guid.Parse("7acad80c-5b8e-4d2d-aa0d-28e467662538"),
            DatePickerUuid = Guid.Parse("10d27e87-0324-4e20-bc01-52586a49c1f0"),
            DateTime = DateTime.Now.AddDays(5),

        };
    }
}