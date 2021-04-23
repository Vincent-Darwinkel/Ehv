using System;
using System.Collections.Generic;
using Datepicker_Service.Models;
using Datepicker_Service.UnitTests.TestModels.FromFrontend;

namespace Datepicker_Service.UnitTests.TestModels
{
    public class TestDatepickerDto
    {
        public readonly DatepickerDto Datepicker = new DatepickerDto
        {
            Uuid = Guid.Parse("9fd56e55-c6cb-4571-8ec4-902ab8aefdd7"),
            AuthorUuid = new TestUser().User.Uuid,
            Dates = new List<DatepickerDateDto> { new TestDatepickerDatesDto().Date },
            Description = "Test Description",
            Expires = new DateTime(2021, 05, 2),
            Location = "Test Location",
            Title = "Test Title"
        };
        public readonly DatepickerDto DatepickerNoUsers = new DatepickerDto
        {
            Uuid = Guid.Parse("9fd56e55-c6cb-4571-8ec4-902ab8aefdd7"),
            AuthorUuid = new TestUser().User.Uuid,
            Dates = new List<DatepickerDateDto> { new TestDatepickerDatesDto().DateNoUsers },
            Description = "Test Description",
            Expires = new DateTime(2021, 05, 2),
            Location = "Test Location",
            Title = "Test Title"
        };
    }
}