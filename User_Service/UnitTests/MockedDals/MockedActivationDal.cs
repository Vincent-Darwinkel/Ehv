using Moq;
using User_Service.Dal.Interfaces;
using User_Service.UnitTests.TestModels;

namespace User_Service.UnitTests.MockedDals
{
    public class MockedDisabledUserDal
    {
        public readonly IDisabledUserDal Mock;

        public MockedDisabledUserDal()
        {
            var testUser = new TestUserDto().User;
            var mock = new Mock<IDisabledUserDal>();

            Mock = mock.Object;
        }
    }
}
