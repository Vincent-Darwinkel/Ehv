using Hobby_Service.Logic;
using UnitTest.HobbyService.MockedDals;

namespace UnitTest.HobbyService.MockedLogics
{
    public class MockedHobbyLogic
    {
        public readonly HobbyLogic HobbyLogic;

        public MockedHobbyLogic()
        {
            var hobbyDal = new MockedHobbyDal().Mock;
            var hobbyLogic = new HobbyLogic(hobbyDal);
            HobbyLogic = hobbyLogic;
        }
    }
}