using Authentication_Service.Logic;
using Authentication_Service.RabbitMq.Publishers;
using Authentication_Service.RabbitMq.Rpc;
using Authentication_Service.UnitTests.MockDals;
using Moq;
using MockedUserDal = Authentication_Service.UnitTests.MockDals.MockedUserDal;

namespace Authentication_Service.UnitTests.MockedLogics
{
    public class MockedAuthenticationLogic
    {
        public readonly AuthenticationLogic AuthenticationLogic;

        public MockedAuthenticationLogic()
        {
            var iUserDalMock = new MockedUserDal().Mock;
            var iPendingLoginDalMock = new MockedPendingLoginDal().Mock;
            var mockedPublisher = new Mock<IPublisher>().Object;
            var mockedRpcClient = new Mock<RpcClient>().Object;
            AuthenticationLogic = new AuthenticationLogic(iUserDalMock, mockedPublisher, iPendingLoginDalMock,
                new SecurityLogic(), new MockedJwtLogic().JwtLogic, mockedRpcClient);
        }
    }
}