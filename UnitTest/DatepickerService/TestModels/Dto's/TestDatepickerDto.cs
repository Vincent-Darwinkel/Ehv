using Datepicker_Service.Models;
using System;
using System.Collections.Generic;
using UnitTest.DatepickerService.TestModels.FromFrontend;

namespace UnitTest.DatepickerService.TestModels
{
    public class TestDatepickerDto
    {
        public readonly DatepickerDto Datepicker = new DatepickerDto
        {
            Uuid = Guid.Parse("9fd56e55-c6cb-4571-8ec4-902ab8aefdd7"),
            AuthorUuid = new TestUser().User.Uuid,
            Dates = new List<DatepickerDateDto> { new TestDatepickerDatesDto().Date },
            Description = "Test Description",
            Expires = DateTime.Now.AddMinutes(5),
            Location = "Test Location",
            Title = "Test Title"
        };
        public readonly DatepickerDto Datepicker2 = new DatepickerDto
        {
            Uuid = Guid.Parse("984c8bbe-cc85-4032-b634-e9209e1fb991"),
            AuthorUuid = new TestUser().User.Uuid,
            Dates = new List<DatepickerDateDto> { new TestDatepickerDatesDto().Date },
            Description = "Test Description",
            Expires = DateTime.Now.AddMinutes(5),
            Location = "Test Location",
            Title = "Test Title 2"
        };
        public readonly DatepickerDto DatepickerNotExisting = new DatepickerDto
        {
            Uuid = Guid.Parse("567fe0de-07e6-48c0-ab4c-3e499c6c29f1"),
            AuthorUuid = new TestUser().User.Uuid,
            Dates = new List<DatepickerDateDto> { new TestDatepickerDatesDto().Date },
            Description = "Test Description",
            Expires = DateTime.Now.AddMinutes(5),
            Location = "Test Location",
            Title = "Datepicker"
        };
        public readonly DatepickerDto DatepickerNoDates = new DatepickerDto
        {
            Uuid = Guid.Parse("39312c7c-99ea-4071-81d4-789cccf90601"),
            AuthorUuid = new TestUser().User.Uuid,
            Dates = new List<DatepickerDateDto>(),
            Description = "Test Description",
            Expires = DateTime.Now.AddMinutes(5),
            Location = "Test Location",
            Title = "Datepicker"
        };
        public readonly DatepickerDto DatepickerDuplicatedEventName = new DatepickerDto
        {
            Uuid = Guid.Parse("b68524ad-4589-4545-b4c3-a96881dc097d"),
            AuthorUuid = new TestUser().User.Uuid,
            Dates = new List<DatepickerDateDto> { new TestDatepickerDatesDto().Date },
            Description = "Test Description",
            Expires = DateTime.Now.AddMinutes(5),
            Location = "Test Location",
            Title = "Test Title"
        };
        public readonly DatepickerDto DatepickerNoUsers = new DatepickerDto
        {
            Uuid = Guid.Parse("9fd56e55-c6cb-4571-8ec4-902ab8aefdd7"),
            AuthorUuid = new TestUser().User.Uuid,
            Dates = new List<DatepickerDateDto> { new TestDatepickerDatesDto().DateNoUsers },
            Description = "Test Description",
            Expires = DateTime.Now.AddDays(2),
            Location = "Test Location",
            Title = "Test Title"
        };
    }
}
