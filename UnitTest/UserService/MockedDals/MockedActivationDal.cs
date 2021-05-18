using Moq;
using UnitTest.UserService.TestModels;
using User_Service.Dal.Interfaces;

namespace UnitTest.UserService.MockedDals
{
    public class MockedDisabledUserDal
    {
        public readonly IDisabledUserDal Mock;

        public MockedDisabledUserDal()
        {
            var testUser = new TestUserDto().User;
            var mock = new Mock<IDisabledUserDal>();
            mock.Setup(dud => dud.Exists(testUser.Uuid)).ReturnsAsync(true);

            Mock = mock.Object;
        }
    }
}
