using Authentication_Service.Logic;
using Authentication_Service.UnitTests.MockDals;

namespace Authentication_Service.UnitTests.MockedLogics
{
    public class MockedJwtLogic
    {
        public readonly JwtLogic JwtLogic;

        public MockedJwtLogic()
        {
            var mockedRefreshTokenDal = new MockedRefreshTokenDal();
            JwtLogic = new JwtLogic(mockedRefreshTokenDal.Mock, null);
        }
    }
}