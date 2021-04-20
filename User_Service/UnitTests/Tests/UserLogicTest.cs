using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using NUnit.Framework;
using User_Service.CustomExceptions;
using User_Service.Logic;
using User_Service.Models;
using User_Service.Models.FromFrontend;
using User_Service.UnitTests.MockedLogics;
using User_Service.UnitTests.TestModels;
using User_Service.UnitTests.TestModels.FromFrontend;

namespace User_Service.UnitTests.Tests
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
            Assert.DoesNotThrowAsync(() => _userLogic.Register(new TestUser().User));
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
        public async Task AllTest()
        {
            Assert.DoesNotThrowAsync(() => _userLogic.All());
        }

        [Test]
        public void UpdateTest()
        {
            UserDto testUser = new TestUserDto().User;
            Assert.DoesNotThrowAsync(() => _userLogic.Update(new User
            {
                Username = testUser.Username,
                Password = "test",
                About = testUser.About,
                Email = testUser.Email,
                Gender = testUser.Gender,
                AccountRole = testUser.AccountRole,
                BirthDate = testUser.BirthDate,
            }, testUser.Uuid));
        }

        [Test]
        public void UpdateUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _userLogic.Update(new User(), Guid.Empty));
        }

        [Test]
        public void UpdateDuplicateNameExceptionTest()
        {
            UserDto testUser = new TestUserDto().User;
            Assert.ThrowsAsync<DuplicateNameException>(() => _userLogic.Update(new User { Username = "test", Email = testUser.Email }, testUser.Uuid));
        }

        [Test]
        public void UpdateKeyNotFoundExceptionTest()
        {
            UserDto testUser = new TestUserDto().User;
            Assert.ThrowsAsync<KeyNotFoundException>(() => _userLogic.Delete(testUser, Guid.Empty));
        }

        [Test]
        public void DeleteSiteAdminTest()
        {
            UserDto testUser = new TestUserDto().User;
            UserDto testSiteAdmin = new TestUserDto().SiteAdmin;
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userLogic.Delete(testSiteAdmin, testUser.Uuid));
        }

        [Test]
        public void DeleteAdminTest()
        {
            UserDto testUser = new TestUserDto().User;
            UserDto testSiteAdmin = new TestUserDto().Admin;
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userLogic.Delete(testSiteAdmin, testUser.Uuid));
        }

        [Test]
        public void DeleteUserTest()
        {
            UserDto testUser = new TestUserDto().User;
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userLogic.Delete(testUser, testUser.Uuid));
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