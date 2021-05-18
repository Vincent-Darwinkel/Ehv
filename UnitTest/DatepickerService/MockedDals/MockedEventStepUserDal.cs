using Event_Service.Dal.Interfaces;
using Moq;
using UnitTest.EventService.TestModels;
using UnitTest.EventService.TestModels.Helpers;

namespace UnitTest.DatepickerService.MockedDals
{
    public class MockedEventStepUserDal
    {
        public readonly IEventStepUserDal Mock;

        public MockedEventStepUserDal()
        {
            var user = new TestUser().User;
            var eventStepUserDto = new TestEventStepUserDto().EventStepUser;
            var eventStepUserDal = new Mock<IEventStepUserDal>();
            eventStepUserDal.Setup(esd => esd.Find(eventStepUserDto.Uuid, user.Uuid)).ReturnsAsync(eventStepUserDto);

            Mock = eventStepUserDal.Object;
        }
    }
}