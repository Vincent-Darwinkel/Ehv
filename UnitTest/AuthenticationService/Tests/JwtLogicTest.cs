using Authentication_Service.CustomExceptions;
using Authentication_Service.Enums;
using Authentication_Service.Logic;
using Authentication_Service.Models.Dto;
using Authentication_Service.Models.ToFrontend;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using UnitTest.AuthenticationService.MockedLogics;
using UnitTest.AuthenticationService.TestModels;

namespace UnitTest.AuthenticationService.Tests
{
    [TestFixture]
    public class JwtLogicTest
    {
        private readonly JwtLogic _jwtLogic;

        public JwtLogicTest()
        {
            _jwtLogic = new MockedJwtLogic().JwtLogic;
        }

        // NOTE these test depends on the createJwt method in jwt logic. If that method fails, most tests in this class will fail

        [Test]
        public async Task CreateJwtTest()
        {
            AuthorizationTokensViewmodel result = await _jwtLogic.CreateJwt(new TestUserDto().User);
            Assert.NotNull(result.RefreshToken);
            Assert.NotNull(result.Jwt);
        }

        [Test]
        public void CreateJwtArgumentNullExceptionTest()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _jwtLogic.CreateJwt(null));
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _jwtLogic.CreateJwt(new UserDto
            {
                Username = "Test",
                AccountRole = AccountRole.User
            }));
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _jwtLogic.CreateJwt(new UserDto
            {
                Uuid = Guid.NewGuid(),
                Username = "Test"
            }));
        }

        [Test]
        public async Task GetClaimGuidTest()
        {
            AuthorizationTokensViewmodel result = await _jwtLogic.CreateJwt(new TestUserDto().User);
            Guid userUuid = _jwtLogic.GetClaim<Guid>(result.Jwt, JwtClaim.Uuid);
            Assert.AreNotEqual(userUuid, Guid.Empty);
        }

        [Test]
        public async Task GetClaimAccountRoleTest()
        {
            AuthorizationTokensViewmodel result = await _jwtLogic.CreateJwt(new TestUserDto().User);
            AccountRole accountRole = _jwtLogic.GetClaim<AccountRole>(result.Jwt, JwtClaim.AccountRole);
            Assert.IsTrue(accountRole == AccountRole.User);
        }

        [Test]
        public void GetClaimUnprocessableExceptionTest()
        {
            Assert.Throws<UnprocessableException>(() => _jwtLogic.GetClaim<Guid>(null, JwtClaim.Uuid));
        }

        [Test]
        public async Task ValidateJwtTest()
        {
            AuthorizationTokensViewmodel result = await _jwtLogic.CreateJwt(new TestUserDto().User);
            TokenValidationResult validationResult = _jwtLogic.ValidateJwt(result.Jwt);
            Assert.True(validationResult.IsValid);
        }

        [Test]
        public void ValidateJwtArgumentNullExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(() => _jwtLogic.ValidateJwt(null));
        }

        [Test]
        public void ValidateJwtNotValidTest()
        {
            TokenValidationResult validationResult = _jwtLogic.ValidateJwt("123");
            Assert.False(validationResult.IsValid);
        }

        [Test]
        public void RefreshJwtUnauthorizedAccessExceptionTest()
        {
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _jwtLogic.RefreshJwt("123", Guid.Empty, null));
        }

        [Test]
        public async Task RefreshJwtSecurityTokenExceptionTest()
        {
            var testUser = new TestUserDto().User;
            AuthorizationTokensViewmodel result = await _jwtLogic.CreateJwt(new TestUserDto().User);
            Assert.ThrowsAsync<SecurityTokenException>(async () => await _jwtLogic.RefreshJwt(result.Jwt, Guid.Empty, testUser));
        }
    }
}
