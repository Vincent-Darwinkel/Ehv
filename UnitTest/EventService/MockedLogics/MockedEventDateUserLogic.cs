using Event_Service.Logic;
using UnitTest.EventService.MockedDals;

namespace UnitTest.EventService.MockedLogics
{
    public class MockedEventDateUserLogic
    {
        public readonly EventDateUserLogic EventDateUserLogic;

        public MockedEventDateUserLogic()
        {
            var eventDateUserDal = new MockedEventDateUserDal().Mock;
            EventDateUserLogic = new EventDateUserLogic(eventDateUserDal);
        }
    }
}