using System;
using System.Collections.Generic;
using Datepicker_Service.Models;

namespace Datepicker_Service.UnitTests.TestModels
{
    public class TestDatepickerDatesDto
    {
        public readonly DatepickerDateDto Date = new DatepickerDateDto
        {
            Uuid = Guid.Parse("2ea6e403-b29f-40f2-97ad-0d6bae90ff92"),
            DatePickerUuid = Guid.Parse("9fd56e55-c6cb-4571-8ec4-902ab8aefdd7"),
            DateTime = new DateTime(2021, 04, 2),
            UserAvailabilities = new List<DatepickerAvailabilityDto>()
        };
        public readonly DatepickerDateDto DateNoUsers = new DatepickerDateDto
        {
            Uuid = Guid.Parse("2ea6e403-b29f-40f2-97ad-0d6bae90ff92"),
            DatePickerUuid = Guid.Parse("9fd56e55-c6cb-4571-8ec4-902ab8aefdd7"),
            DateTime = new DateTime(2021, 04, 2),
        };
    }
}