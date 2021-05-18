using Event_Service.Dal.Interfaces;
using Moq;
using UnitTest.EventService.TestModels;

namespace UnitTest.DatepickerService.MockedDals
{
    public class MockedEventStepDal
    {
        public readonly IEventStepDal Mock;

        public MockedEventStepDal()
        {
            var eventStepDto = new TestEventStepDto().EventStep;
            var eventStepNoUsersDto = new TestEventStepDto().EventStepNoUsers;
            var eventStepDal = new Mock<IEventStepDal>();
            eventStepDal.Setup(esd => esd.Find(eventStepDto.Uuid)).ReturnsAsync(eventStepDto);
            eventStepDal.Setup(esd => esd.Find(eventStepNoUsersDto.Uuid)).ReturnsAsync(eventStepNoUsersDto);

            Mock = eventStepDal.Object;
        }
    }
}