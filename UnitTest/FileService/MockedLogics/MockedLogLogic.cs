using File_Service.Logic;
using File_Service.RabbitMq.Publishers;
using Moq;

namespace UnitTest.FileService.MockedLogics
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