using Authentication_Service.Logic;
using Authentication_Service.UnitTests.MockDals;
using Authentication_Service.UnitTests.MockedDotnetClasses;

namespace Authentication_Service.UnitTests.MockedLogics
{
    public class MockedJwtLogic
    {
        public readonly JwtLogic JwtLogic;

        public MockedJwtLogic()
        {
            var mockedRefreshTokenDal = new RefreshTokenDalMock();
            var mockedConfiguration = new MockOptions();
            JwtLogic = new JwtLogic(mockedRefreshTokenDal.Mock, mockedConfiguration.JwtConfig);
        }
    }
}