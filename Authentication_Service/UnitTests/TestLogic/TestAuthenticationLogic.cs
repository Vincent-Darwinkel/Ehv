using Authentication_Service.Logic;
using Authentication_Service.UnitTests.MockDals;

namespace Authentication_Service.UnitTests.TestLogic
{
    public class TestAuthenticationLogic
    {
        private readonly AuthenticationLogic _authenticationLogic;

        public TestAuthenticationLogic()
        {
            var iUserDalMock = new UserDalMock();
            _authenticationLogic = new AuthenticationLogic(iUserDalMock.Mock);
        }
    }
}