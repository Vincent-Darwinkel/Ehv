using Event_Service.Logic;
using NUnit.Framework;
using System.Data;
using UnitTest.EventService.MockedLogics;
using UnitTest.EventService.TestModels;
using UnitTest.EventService.TestModels.Helpers;

namespace UnitTest.EventService.Tests
{
    [TestFixture]
    public class EventDateUserLogicTest
    {
        private readonly EventDateUserLogic _eventDateUserLogic;

        public EventDateUserLogicTest()
        {
            _eventDateUserLogic = new MockedEventDateUserLogic().EventDateUserLogic;
        }

        [Test]
        public void RemoveTest()
        {
            var eventDate = new TestEventDate().Date;
            var user = new TestUser().User;
            Assert.DoesNotThrowAsync(() => _eventDateUserLogic.Remove(eventDate.Uuid, user));
        }

        [Test]
        public void RemoveNoNullAllowedExceptionTest()
        {
            var eventDate = new TestEventDate().DateNotLinked;
            var user = new TestUser().User;
            Assert.ThrowsAsync<NoNullAllowedException>(() => _eventDateUserLogic.Remove(eventDate.Uuid, user));
        }
    }
}