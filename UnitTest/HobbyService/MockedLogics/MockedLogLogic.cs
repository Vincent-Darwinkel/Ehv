using Hobby_Service.Logic;
using Hobby_Service.RabbitMq.Publishers;
using Moq;

namespace UnitTest.HobbyService.MockedLogics
{
    public class MockedLogLogic
    {
        public readonly LogLogic LogLogic;

        public MockedLogLogic()
        {
            var mockedPublisher = new Mock<IPublisher>().Object;
            LogLogic = new LogLogic(mockedPublisher);
        }
    }
}