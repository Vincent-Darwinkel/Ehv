using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTest.UserService.MockedLogics;
using UnitTest.UserService.TestModels;
using UnitTest.UserService.TestModels.FromFrontend;
using User_Service.CustomExceptions;
using User_Service.Logic;
using User_Service.Models;
using User_Service.Models.FromFrontend;

namespace UnitTest.UserService.Tests
{
    [TestFixture]
    public class UserLogicTest
    {
        private readonly UserLogic _userLogic;

        public UserLogicTest()
        {
            _userLogic = new MockedUserLogic().UserLogic;
        }

        [Test]
        public void RegisterTest()
        {
            Assert.DoesNotThrowAsync(() => _userLogic.Register(new TestUser().NewUser));
        }

        [Test]
        public void RegisterUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _userLogic.Register(new User()));
        }

        [Test]
        public async Task FindTest()
        {
            var testUser = new TestUserDto().User;
            Assert.IsTrue((await _userLogic.Find(testUser.Uuid)).Uuid != Guid.Empty);
            Assert.IsTrue((await _userLogic.Find(new List<Guid> { testUser.Uuid })).Count >= 1);
        }

        [Test]
        public void FindRabbitMqTest()
        {
            var user = new TestUserDto().User;
            var userUuidCollection = new List<Guid> { user.Uuid };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(userUuidCollection);
            Assert.DoesNotThrowAsync(() => _userLogic.Find(json));
        }

        [Test]
        public void AllTest()
        {
            Assert.DoesNotThrowAsync(() => _userLogic.All());
        }

        [Test]
        public void UpdateUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _userLogic.Update(new User(), Guid.Empty));
        }

        [Test]
        public void UpdateUnauthorizedAccessExceptionTest()
        {
            var testUser = new TestUser().User;
            var testUserDto = new TestUserDto().User;
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userLogic.Update(testUser, testUserDto.Uuid));
        }

        [Test]
        public void UpdateKeyNotFoundExceptionTest()
        {
            UserDto testUser = new TestUserDto().User;
            Assert.ThrowsAsync<KeyNotFoundException>(() => _userLogic.Delete(testUser, Guid.Empty));
        }

        [Test]
        public void DeleteSiteAdminSiteAdminRequiredTest()
        {
            UserDto testUser = new TestUserDto().User;
            UserDto testSiteAdmin = new TestUserDto().SiteAdmin;
            Assert.ThrowsAsync<SiteAdminRequiredException>(() => _userLogic.Delete(testSiteAdmin, testUser.Uuid));
        }

        [Test]
        public void DeleteAdminTest()
        {
            UserDto testUser = new TestUserDto().User;
            UserDto testSiteAdmin = new TestUserDto().Admin;
            Assert.DoesNotThrowAsync(() => _userLogic.Delete(testSiteAdmin, testUser.Uuid));
        }

        [Test]
        public void DeleteUserTest()
        {
            UserDto testUser = new TestUserDto().User;
            Assert.DoesNotThrowAsync(() => _userLogic.Delete(testUser, testUser.Uuid));
        }

        [Test]
        public void DeleteUnauthorizedAccessExceptionTest()
        {
            UserDto testUser = new TestUserDto().User;
            UserDto testSiteAdmin = new TestUserDto().SiteAdmin;
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userLogic.Delete(testUser, testSiteAdmin.Uuid));
        }
    }
}
