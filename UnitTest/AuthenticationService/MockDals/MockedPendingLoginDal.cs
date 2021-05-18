using Authentication_Service.Dal.Interface;
using Moq;
using UnitTest.AuthenticationService.TestModels;

namespace UnitTest.AuthenticationService.MockDals
{
    public class MockedPendingLoginDal
    {
        public readonly IPendingLoginDal Mock;

        public MockedPendingLoginDal()
        {
            var testPendingLogin = new TestPendingLoginDto();
            var mock = new Mock<IPendingLoginDal>();
            mock.Setup(pld => pld.Find(testPendingLogin.SiteAdmin)).ReturnsAsync(testPendingLogin.SiteAdmin);
            Mock = mock.Object;
        }
    }
}
