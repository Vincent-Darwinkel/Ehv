using NUnit.Framework;
using System;
using System.Threading.Tasks;
using UnitTest.UserService.MockedLogics;
using UnitTest.UserService.TestModels;
using UnitTest.UserService.TestModels.FromFrontend;
using UnitTest.UserService.TestModels.RabbitMq;
using User_Service.CustomExceptions;
using User_Service.Logic;
using User_Service.Models.FromFrontend;

namespace UnitTest.UserService.Tests
{
    [TestFixture]
    public class DisabledUserLogicTest
    {
        private readonly DisabledUserLogic _disabledUserLogic;

        public DisabledUserLogicTest()
        {
            _disabledUserLogic = new MockedDisabledUserLogic().DisabledUserLogic;
        }

        [Test]
        public void AddTest()
        {
            var disabledUser = new TestDisabledUser().DisabledUser;
            Assert.DoesNotThrowAsync(() => _disabledUserLogic.Add(disabledUser));
        }

        [Test]
        public void AddUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _disabledUserLogic.Add(new DisabledUser()));
        }

        [Test]
        public void AddRabbitMqTest()
        {
            var disabledUser = new TestDisabledUserRabbitMq().User;
            Assert.DoesNotThrowAsync(() => _disabledUserLogic.Add(disabledUser));
        }

        [Test]
        public void AddRabbitMqUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _disabledUserLogic.Add(new DisabledUser()));
        }

        [Test]
        public void AllTest()
        {
            Assert.DoesNotThrowAsync(() => _disabledUserLogic.All());
        }

        [Test]
        public async Task ExistsTest()
        {
            var testUser = new TestUserDto().User;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(testUser.Uuid);
            bool exists = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(await _disabledUserLogic.Exists(json));
            Assert.IsTrue(exists);
        }

        [Test]
        public void ExistsUnprocessableExceptionTest()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(Guid.Empty);
            Assert.ThrowsAsync<UnprocessableException>(() => _disabledUserLogic.Exists(json));

        }

        [Test]
        public void DeleteTest()
        {
            var testUser = new TestUserDto().User;
            Assert.DoesNotThrowAsync(() => _disabledUserLogic.Delete(testUser.Uuid));
        }

        [Test]
        public void DeleteUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _disabledUserLogic.Delete(Guid.Empty));
        }
    }
}