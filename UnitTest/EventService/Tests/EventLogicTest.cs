using Event_Service.CustomExceptions;
using Event_Service.Enums;
using Event_Service.Logic;
using Event_Service.Models.HelperFiles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnitTest.EventService.MockedLogics;
using UnitTest.EventService.TestModels;
using UnitTest.EventService.TestModels.RabbitMq;
using TestUser = UnitTest.EventService.TestModels.Helpers.TestUser;

namespace UnitTest.EventService.Tests
{
    [TestFixture]
    public class EventLogicTest
    {
        private readonly EventLogic _eventLogic;

        public EventLogicTest()
        {
            _eventLogic = new MockedEventLogic().EventLogic;
        }

        [Test]
        public void AllTest()
        {
            var user = new TestUser().User;
            Assert.DoesNotThrowAsync(() => _eventLogic.All(user));
        }

        [Test]
        public void FindTest()
        {
            var testEvent = new TestEventDto().Event;
            var user = new TestUser().User;
            Assert.DoesNotThrowAsync(() => _eventLogic.Find(testEvent.Uuid, user));
        }

        [Test]
        public void FindUnprocessableExceptionTest()
        {
            var user = new TestUser().User;
            Assert.ThrowsAsync<UnprocessableException>(() => _eventLogic.Find(Guid.Empty, user));
        }

        [Test]
        public void FindKeyNotFoundExceptionTest()
        {
            var user = new TestUser().User;
            Assert.ThrowsAsync<KeyNotFoundException>(() => _eventLogic.Find(Guid.Parse("e9cc8435-3210-429c-a032-934efa2d5bd7"), user));
        }

        [Test]
        public void ConvertToEventTest()
        {
            var datepickerConversion = new TestDatepickerRabbitMq().DatepickerRabbit;
            Assert.DoesNotThrowAsync(() => _eventLogic.ConvertToEventAsync(datepickerConversion));
        }

        [Test]
        public async Task ExistsTest()
        {
            var testEvent = new TestEventDto().Event;
            Assert.DoesNotThrowAsync(() => _eventLogic.Exists(testEvent.Title));
        }

        [Test]
        public async Task ExistsReturnsTrueTest()
        {
            var testEvent = new TestEventDto().Event;
            string result = await _eventLogic.Exists(testEvent.Title);

            bool exists = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(result);
            Assert.IsTrue(exists);
        }

        [Test]
        public void DeleteTest()
        {
            var testEvent = new TestEventDto().Event;
            var user = new TestUser().User;
            Assert.DoesNotThrowAsync(() => _eventLogic.Delete(testEvent.Uuid, user));
        }

        [Test]
        public void DeleteUnauthorizedAccessExceptionTest()
        {
            var testEvent = new TestEventDto().Event;
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _eventLogic.Delete(testEvent.Uuid, new UserHelper
            {
                AccountRole = AccountRole.User
            }));
        }

        [Test]
        public void DeleteTestNoNullAllowedExceptionTest()
        {
            var user = new TestUser().User;
            Assert.ThrowsAsync<NoNullAllowedException>(() => _eventLogic.Delete(Guid.Empty, user));
        }

        [Test]
        public void DeleteTestNoNullAllowedExceptionTwoTest()
        {
            var user = new TestUser().User;
            Assert.ThrowsAsync<NoNullAllowedException>(() => _eventLogic.Delete(Guid.Parse("a654024c-e3f0-4fd0-add9-02137b0f26f1"), user));
        }
    }
}