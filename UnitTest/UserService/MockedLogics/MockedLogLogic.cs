using Moq;
using User_Service.Logic;
using User_Service.RabbitMq.Publishers;

namespace UnitTest.UserService.MockedLogics
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