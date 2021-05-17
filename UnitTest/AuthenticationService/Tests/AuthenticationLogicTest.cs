using Authentication_Service.Enums;
using Authentication_Service.Logic;
using Authentication_Service.Models.FromFrontend;
using Authentication_Service.Models.ToFrontend;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using UnitTest.AuthenticationService.MockedLogics;
using UnitTest.AuthenticationService.TestModels;
using UnitTest.AuthenticationService.TestModels.TestFromFrontend;

namespace UnitTest.AuthenticationService.Tests
{
    [TestFixture]
    public class AuthenticationLogicTest
    {
        private readonly AuthenticationLogic _authenticationLogic;

        public AuthenticationLogicTest()
        {
            _authenticationLogic = new MockedAuthenticationLogic().AuthenticationLogic;
        }

        [Test]
        public async Task LoginTest()
        {
            LoginResultViewmodel result = await _authenticationLogic.Login(new TestLogin().Login);
            Assert.AreNotEqual(new LoginResultViewmodel(), result);
            Assert.NotNull(result.RefreshToken);
            Assert.NotNull(result.Jwt);
        }

        [Test]
        public void LoginTestUnauthorized()
        {
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _authenticationLogic.Login(new Login()));
        }

        [Test]
        public async Task LoginMultiRole()
        {
            var testSiteAdmin = new TestUserDto().SiteAdmin;
            var login = new Login
            {
                Username = testSiteAdmin.Username,
                Password = "test"
            };

            var result = await _authenticationLogic.Login(login);
            Assert.IsTrue(result.SelectableAccountRoles.Count == 3);
            Assert.IsTrue(result.UserHasMultipleAccountRoles);
        }

        [Test]
        public void LoginWithSelectedAccountRoleUnauthorizedAccessException()
        {
            var testSiteAdmin = new TestUserDto().SiteAdmin;
            var login = new Login
            {
                Username = testSiteAdmin.Username,
                Password = "test",
                SelectedAccountRole = AccountRole.SiteAdmin,
                LoginCode = 342534
            };

            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _authenticationLogic.Login(login));
        }
    }
}
