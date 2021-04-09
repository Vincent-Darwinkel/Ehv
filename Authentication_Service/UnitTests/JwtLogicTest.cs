using System;
using System.Threading.Tasks;
using Authentication_Service.CustomExceptions;
using Authentication_Service.Enums;
using Authentication_Service.Logic;
using Authentication_Service.Models.Dto;
using Authentication_Service.Models.ToFrontend;
using Authentication_Service.UnitTests.MockedLogics;
using Authentication_Service.UnitTests.TestModels;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace Authentication_Service.UnitTests
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
            LoginResultViewmodel result = await _jwtLogic.CreateJwt(new TestUserDto().User);
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
                UserUuid = Guid.NewGuid(),
                Username = "Test"
            }));
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _jwtLogic.CreateJwt(new UserDto
            {
                UserUuid = Guid.NewGuid(),
                AccountRole = AccountRole.User
            }));
        }

        [Test]
        public async Task GetClaimGuidTest()
        {
            LoginResultViewmodel result = await _jwtLogic.CreateJwt(new TestUserDto().User);
            Guid userUuid = _jwtLogic.GetClaim<Guid>(result.Jwt, JwtClaim.UserUuid);
            Assert.AreNotEqual(userUuid, Guid.Empty);
        }

        [Test]
        public async Task GetClaimAccountRoleTest()
        {
            LoginResultViewmodel result = await _jwtLogic.CreateJwt(new TestUserDto().User);
            AccountRole accountRole = _jwtLogic.GetClaim<AccountRole>(result.Jwt, JwtClaim.AccountRole);
            Assert.Equals(accountRole, AccountRole.User);
        }

        [Test]
        public async Task GetClaimUsernameTest()
        {
            LoginResultViewmodel result = await _jwtLogic.CreateJwt(new TestUserDto().User);
            string username = _jwtLogic.GetClaim<string>(result.Jwt, JwtClaim.Username);
            Assert.NotNull(username);
        }

        [Test]
        public void GetClaimUnprocessableExceptionTest()
        {
            Assert.Throws<UnprocessableException>(() => _jwtLogic.GetClaim<Guid>(null, JwtClaim.UserUuid));
        }

        [Test]
        public async Task ValidateJwtTest()
        {
            LoginResultViewmodel result = await _jwtLogic.CreateJwt(new TestUserDto().User);
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
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _jwtLogic.RefreshJwt("123", null, null));
        }

        [Test]
        public async Task RefreshJwtSecurityTokenExceptionTest()
        {
            var testUser = new TestUserDto().User;
            LoginResultViewmodel result = await _jwtLogic.CreateJwt(new TestUserDto().User);
            Assert.ThrowsAsync<SecurityTokenException>(async () => await _jwtLogic.RefreshJwt(result.Jwt, null, testUser));
        }
    }
}