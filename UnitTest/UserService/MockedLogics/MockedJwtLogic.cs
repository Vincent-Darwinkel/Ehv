using User_Service.Logic;

namespace UnitTest.UserService.MockedLogics
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