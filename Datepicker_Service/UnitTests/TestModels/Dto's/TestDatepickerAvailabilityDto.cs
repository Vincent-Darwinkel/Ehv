using Datepicker_Service.Models;
using System;

namespace Datepicker_Service.UnitTests.TestModels
{
    public class TestDatepickerAvailabilityDto
    {
        public readonly DatepickerAvailabilityDto DatepickerAvailability = new DatepickerAvailabilityDto()
        {
            DateUuid = Guid.Parse("2ea6e403-b29f-40f2-97ad-0d6bae90ff92"),
            UserUuid = Guid.Parse("39f2068c-7839-413c-bdfa-0c03ecdce729"),
            Uuid = Guid.Parse("acbc8e8e-68c4-4622-acd5-10f25f3a2861")
        };
    }
}