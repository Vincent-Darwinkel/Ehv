using Authentication_Service.Dal.Interface;
using Authentication_Service.UnitTests.TestModels;
using Moq;

namespace Authentication_Service.UnitTests.MockDals
{
    public class MockedUserDal
    {
        public readonly IUserDal Mock;

        public MockedUserDal()
        {
            var testUser = new TestUserDto().User;
            var mock = new Mock<IUserDal>();
            mock.Setup(m => m.Find(testUser.Username)).ReturnsAsync(testUser);

            Mock = mock.Object;
        }
    }
}