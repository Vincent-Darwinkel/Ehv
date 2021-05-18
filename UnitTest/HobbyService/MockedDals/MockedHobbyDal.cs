using Hobby_Service.Dal.Interfaces;
using Moq;

namespace UnitTest.HobbyService.MockedDals
{
    public class MockedHobbyDal
    {
        public readonly IHobbyDal Mock;

        public MockedHobbyDal()
        {
            var hobbyDal = new Mock<IHobbyDal>();
            Mock = hobbyDal.Object;
        }
    }
}