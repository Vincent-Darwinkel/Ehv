using NUnit.Framework;
using System.Collections.Generic;
using UnitTest.UserService.MockedLogics;
using UnitTest.UserService.TestModels.RabbitMq;
using User_Service.CustomExceptions;
using User_Service.Logic;
using User_Service.Models.RabbitMq;

namespace UnitTest.UserService.Tests
{
    [TestFixture]
    public class ActivationLogicTest
    {
        private readonly ActivationLogic _activationLogic;

        public ActivationLogicTest()
        {
            _activationLogic = new MockedActivationLogic().ActivationLogic;
        }

        [Test]
        public void AddUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _activationLogic.Add(new UserActivationRabbitMq()));
        }

        [Test]
        public void AddTest()
        {
            var activation = new TestUserActivationRabbitMq().UserActivation;
            Assert.DoesNotThrowAsync(() => _activationLogic.Add(activation));
        }

        [Test]
        public void ActivationUserTest()
        {
            var userActivationRabbitMq = new TestUserActivationRabbitMq().UserActivation;
            Assert.DoesNotThrowAsync(() => _activationLogic.ActivateUser(userActivationRabbitMq.Code));
        }

        [Test]
        public void ActivationUserKeyNotFoundExceptionTest()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(() => _activationLogic.ActivateUser("test"));
        }
    }
}