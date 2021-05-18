using User_Service.Enums;
using User_Service.Models.FromFrontend;

namespace UnitTest.UserService.TestModels.FromFrontend
{
    public class TestDisabledUser
    {
        public readonly DisabledUser DisabledUser = new DisabledUser
        {
            Reason = DisableReason.AccountViolation,
            UserUuid = new TestUserDto().User.Uuid,
        };
    }
}