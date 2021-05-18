using Authentication_Service.Logic;
using UnitTest.AuthenticationService.MockDals;
using UnitTest.AuthenticationService.MockedDotnetClasses;

namespace UnitTest.AuthenticationService.MockedLogics
{
    public class MockedJwtLogic
    {
        public readonly JwtLogic JwtLogic;

        public MockedJwtLogic()
        {
            var mockedRefreshTokenDal = new MockedRefreshTokenDal();
            var jwtConfig = new MockedJwtConfig().JwtConfig;
            JwtLogic = new JwtLogic(mockedRefreshTokenDal.Mock, jwtConfig);
        }
    }
}