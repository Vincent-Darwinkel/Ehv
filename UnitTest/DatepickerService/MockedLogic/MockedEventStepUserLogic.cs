using Event_Service.Logic;
using UnitTest.DatepickerService.MockedDals;

namespace UnitTest.DatepickerService.MockedLogic
{
    public class MockedEventStepUserLogic
    {
        public readonly EventStepUserLogic EventStepUserLogic;

        public MockedEventStepUserLogic()
        {
            var eventStepUserDal = new MockedEventStepUserDal().Mock;
            var eventStepDal = new MockedEventStepDal().Mock;
            EventStepUserLogic = new EventStepUserLogic(eventStepUserDal, eventStepDal);
        }
    }
}