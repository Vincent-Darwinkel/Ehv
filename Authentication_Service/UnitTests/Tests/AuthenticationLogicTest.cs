using Authentication_Service.Logic;
using Authentication_Service.Models.FromFrontend;
using Authentication_Service.Models.ToFrontend;
using Authentication_Service.UnitTests.MockedLogics;
using Authentication_Service.UnitTests.TestModels.TestFromFrontend;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Authentication_Service.UnitTests.Tests
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
    }
}
