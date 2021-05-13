using Authentication_Service.Logic;
using Authentication_Service.RabbitMq.Publishers;
using Authentication_Service.RabbitMq.Rpc;
using Authentication_Service.UnitTests.MockDals;
using Moq;

namespace Authentication_Service.UnitTests.MockedLogics
{
    public class MockedUserLogic
    {
        public readonly UserLogic UserLogic;

        public MockedUserLogic()
        {
            var mockedUserDal = new MockedUserDal().Mock;
            var mockedDisabledUserDal = new MockedDisabledUserDal().Mock;
            var mockedActivationDal = new MockedActivationDal().Mock;
            var mockedPublisher = new Mock<IPublisher>().Object;
            var mockedRpcServer = new Mock<RpcClient>().Object;
            UserLogic = new UserLogic(mockedUserDal, mockedDisabledUserDal, mockedActivationDal, new SecurityLogic(), mockedPublisher, mockedRpcServer);
        }
    }
}
