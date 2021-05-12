using Authentication_Service.Logic;

namespace Authentication_Service.UnitTests.MockedLogics
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