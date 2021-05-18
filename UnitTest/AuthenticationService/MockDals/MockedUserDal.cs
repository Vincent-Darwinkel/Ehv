using Authentication_Service.Dal.Interface;
using Moq;
using UnitTest.AuthenticationService.TestModels;

namespace UnitTest.AuthenticationService.MockDals
{
    public class MockedUserDal
    {
        public readonly IUserDal Mock;

        public MockedUserDal()
        {
            var testUser = new TestUserDto().User;
            var testSiteAdmin = new TestUserDto().SiteAdmin;

            var mock = new Mock<IUserDal>();
            mock.Setup(m => m.Find(testUser.Username)).ReturnsAsync(testUser);
            mock.Setup(m => m.Find(testUser.Uuid)).ReturnsAsync(testUser);
            mock.Setup(m => m.Find(testSiteAdmin.Username)).ReturnsAsync(testSiteAdmin);

            Mock = mock.Object;
        }
    }
}
