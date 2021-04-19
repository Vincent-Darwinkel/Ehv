using Authentication_Service.Logic;
using Authentication_Service.UnitTests.MockDals;

namespace Authentication_Service.UnitTests.MockedLogics
{
    public class MockedUserLogic
    {
        public readonly UserLogic UserLogic;

        public MockedUserLogic()
        {
            var mockedUserDal = new MockedUserDal().Mock;
            UserLogic = new UserLogic(mockedUserDal);
        }
    }
}