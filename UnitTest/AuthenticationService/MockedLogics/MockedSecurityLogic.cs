using Authentication_Service.Logic;

namespace UnitTest.AuthenticationService.MockedLogics
{
    public class MockedSecurityLogic
    {
        public readonly SecurityLogic SecurityLogic;

        public MockedSecurityLogic()
        {
            SecurityLogic = new SecurityLogic();
        }
    }
}