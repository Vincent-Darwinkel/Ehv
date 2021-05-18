using Event_Service.Dal.Interfaces;
using Moq;
using UnitTest.EventService.TestModels;
using UnitTest.EventService.TestModels.Helpers;

namespace UnitTest.EventService.MockedDals
{
    public class MockedEventDateUserDal
    {
        public readonly IEventDateUserDal Mock;

        public MockedEventDateUserDal()
        {
            var testEventDateUser = new TestEventDateUsersDto().EventDateUser;
            var testUser = new TestUser().User;
            var eventDateUserDal = new Mock<IEventDateUserDal>();
            eventDateUserDal.Setup(edu => edu.Find(testEventDateUser.EventDateUuid, testUser.Uuid)).ReturnsAsync(testEventDateUser);

            Mock = eventDateUserDal.Object;
        }
    }
}