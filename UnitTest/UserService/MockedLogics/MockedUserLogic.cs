using Moq;
using UnitTest.UserService.MockedDals;
using User_Service.Logic;
using User_Service.Models.HelperFiles;
using User_Service.RabbitMq.Publishers;
using User_Service.RabbitMq.Rpc;

namespace UnitTest.UserService.MockedLogics
{
    public class MockedUserLogic
    {
        public readonly UserLogic UserLogic;

        public MockedUserLogic()
        {
            var mockedUserDal = new MockedUserDal().Mock;
            var mockedProducer = new Mock<IPublisher>();
            var mockedActivationDal = new MockedActivationDal().Mock;
            var mockedDisabledUserDal = new MockedDisabledUserDal().Mock;
            var autoMapperConfig = AutoMapperConfig.Config.CreateMapper();
            var rpcClient = new Mock<IRpcClient>();
            UserLogic = new UserLogic(mockedUserDal, mockedDisabledUserDal, mockedActivationDal, autoMapperConfig,
                mockedProducer.Object, rpcClient.Object);
        }
    }
}
