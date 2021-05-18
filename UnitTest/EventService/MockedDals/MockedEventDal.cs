using Event_Service.Dal.Interfaces;
using Moq;
using UnitTest.EventService.TestModels;

namespace UnitTest.EventService.MockedDals
{
    public class MockedEventDal
    {
        public readonly IEventDal Mock;

        public MockedEventDal()
        {
            var testEvent = new TestEventDto().Event;
            var eventDal = new Mock<IEventDal>();
            eventDal.Setup(ed => ed.Find(testEvent.Uuid)).ReturnsAsync(testEvent);
            eventDal.Setup(ed => ed.Exists(testEvent.Title)).ReturnsAsync(true);

            Mock = eventDal.Object;
        }
    }
}