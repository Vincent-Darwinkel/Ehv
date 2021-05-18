using Authentication_Service.CustomExceptions;
using Authentication_Service.Logic;
using Authentication_Service.Models.Dto;
using Authentication_Service.Models.RabbitMq;
using NUnit.Framework;
using System;
using UnitTest.AuthenticationService.MockedLogics;
using UnitTest.AuthenticationService.TestModels;

namespace UnitTest.AuthenticationService.Tests
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
        public void AddUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _userLogic.Add(new UserRabbitMqSensitiveInformation()));
        }

        [Test]
        public void UpdateTest()
        {
            Assert.DoesNotThrowAsync(() => _userLogic.Update(new TestUserDto().User));
        }

        [Test]
        public void UpdateUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _userLogic.Update(new UserDto()));
        }

        [Test]
        public void DeleteTest()
        {
            Assert.DoesNotThrowAsync(() => _userLogic.Delete(Guid.NewGuid()));
        }

        [Test]
        public void DeleteUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _userLogic.Delete(Guid.Empty));
        }
    }
}