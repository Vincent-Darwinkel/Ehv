using Favorite_Artist_Service.Logic;
using Favorite_Artist_Service.RabbitMq.Publishers;
using Moq;

namespace UnitTest.ArtistService.MockedLogics
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