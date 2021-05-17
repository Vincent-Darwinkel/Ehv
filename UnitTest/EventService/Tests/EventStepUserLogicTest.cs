using Event_Service.Logic;
using NUnit.Framework;
using System.Data;
using UnitTest.DatepickerService.MockedLogic;
using UnitTest.EventService.TestModels;
using UnitTest.EventService.TestModels.Helpers;

namespace UnitTest.EventService.Tests
{
    [TestFixture]
    public class EventStepUserLogicTest
    {
        private readonly EventStepUserLogic _eventStepUserLogic;

        public EventStepUserLogicTest()
        {
            _eventStepUserLogic = new MockedEventStepUserLogic().EventStepUserLogic;
        }

        [Test]
        public void AddTest()
        {
            var eventStep = new TestEventStepDto().EventStep;
            var user = new TestUser().User;
            Assert.DoesNotThrowAsync(() => _eventStepUserLogic.Add(eventStep.Uuid, user));
        }

        [Test]
        public void AddDuplicateNameExceptionTest()
        {
            var eventStepUserDto = new TestEventStepUserDto().EventStepUser;
            var user = new TestUser().User;
            Assert.ThrowsAsync<DuplicateNameException>(() => _eventStepUserLogic.Add(eventStepUserDto.Uuid, user));
        }

        [Test]
        public void DeleteNoNullAllowedExceptionTest()
        {
            var eventStep = new TestEventStepDto().EventStepNoUsers;
            var user = new TestUser().User;
            Assert.ThrowsAsync<NoNullAllowedException>(() => _eventStepUserLogic.Delete(eventStep.Uuid, user));
        }

        [Test]
        public void DeleteTest()
        {
            var eventStep = new TestEventStepDto().EventStep;
            var user = new TestUser().User;
            Assert.DoesNotThrowAsync(() => _eventStepUserLogic.Delete(eventStep.Uuid, user));

        }
    }
}