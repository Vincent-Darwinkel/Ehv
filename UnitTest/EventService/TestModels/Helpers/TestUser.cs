using Event_Service.Enums;
using Event_Service.Models.HelperFiles;
using System;

namespace UnitTest.EventService.TestModels.Helpers
{
    public class TestUser
    {
        public readonly UserHelper User = new UserHelper
        {
            AccountRole = AccountRole.Admin,
            Uuid = Guid.Parse("9a5f0e85-658a-4a96-9e99-3d693d7175bc")
        };
    }
}