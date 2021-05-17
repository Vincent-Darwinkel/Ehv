using Event_Service.Logic;
using Event_Service.Models.HelperFiles;
using Event_Service.RabbitMq.Publishers;
using Event_Service.RabbitMq.Rpc;
using Moq;
using UnitTest.EventService.MockedDals;

namespace UnitTest.EventService.MockedLogics
{
    public class MockedEventLogic
    {
        public readonly EventLogic EventLogic;

        public MockedEventLogic()
        {
            var eventDal = new MockedEventDal().Mock;
            var autoMapperConfig = AutoMapperConfig.Config.CreateMapper();
            var rpcClient = new Mock<IRpcClient>();
            var publisher = new Mock<IPublisher>();

            EventLogic = new EventLogic(eventDal, autoMapperConfig, rpcClient.Object, publisher.Object);
        }
    }
}