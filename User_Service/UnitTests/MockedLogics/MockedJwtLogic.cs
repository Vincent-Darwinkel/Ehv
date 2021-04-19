using User_Service.Logic;

namespace User_Service.UnitTests.MockedLogics
{
    public class MockedJwtLogic
    {
        public readonly JwtLogic JwtLogic;

        public MockedJwtLogic()
        {
            var jwtLogic = new JwtLogic();
            JwtLogic = jwtLogic;
        }
    }
}