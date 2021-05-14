using System;
using Datepicker_Service.Enums;
using Datepicker_Service.Models.HelperFiles;

namespace Datepicker_Service.UnitTests.TestModels.FromFrontend
{
    public class TestUser
    {
        public readonly UserHelper User = new UserHelper
        {
            AccountRole = AccountRole.User,
            Uuid = Guid.Parse("39f2068c-7839-413c-bdfa-0c03ecdce729")
        };
    }
}