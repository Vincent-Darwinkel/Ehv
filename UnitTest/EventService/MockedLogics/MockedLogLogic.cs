using Event_Service.Logic;
using Event_Service.RabbitMq.Publishers;
using Moq;

namespace UnitTest.EventService.MockedLogics
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