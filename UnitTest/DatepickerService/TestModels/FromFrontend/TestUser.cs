using Datepicker_Service.Enums;
using Datepicker_Service.Models.HelperFiles;
using System;

namespace UnitTest.DatepickerService.TestModels.FromFrontend
{
    public class TestUser
    {
        public readonly UserHelper User = new UserHelper
        {
            AccountRole = AccountRole.User,
            Uuid = Guid.Parse("39f2068c-7839-413c-bdfa-0c03ecdce729")
        };
        public readonly UserHelper SiteAdmin = new UserHelper
        {
            AccountRole = AccountRole.User,
            Uuid = Guid.Parse("b9ec2169-aee6-4e17-a2f5-2e0f8db025d9")
        };
    }
}