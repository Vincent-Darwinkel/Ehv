using Authentication_Service.Logic;
using Authentication_Service.UnitTests.MockDals;

namespace Authentication_Service.UnitTests.MockedLogics
{
    public class MockedAuthenticationLogic
    {
        public readonly AuthenticationLogic AuthenticationLogic;

        public MockedAuthenticationLogic()
        {
            var iUserDalMock = new MockedUserDal();
            AuthenticationLogic = new AuthenticationLogic(iUserDalMock.Mock, new SecurityLogic(), new MockedJwtLogic().JwtLogic);
        }
    }
}