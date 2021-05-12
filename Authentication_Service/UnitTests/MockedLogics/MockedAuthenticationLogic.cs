using Authentication_Service.Logic;
using Authentication_Service.RabbitMq.Publishers;
using Authentication_Service.UnitTests.MockDals;
using Moq;

namespace Authentication_Service.UnitTests.MockedLogics
{
    public class MockedAuthenticationLogic
    {
        public readonly AuthenticationLogic AuthenticationLogic;

        public MockedAuthenticationLogic()
        {
            var iUserDalMock = new MockedUserDal().Mock;
            var iPendingLoginDalMock = new MockedPendingLoginDal().Mock;
            var iDisabledUserDalMock = new MockedDisabledUserDal().Mock;
            var mockedPublisher = new Mock<IPublisher>().Object;
            AuthenticationLogic = new AuthenticationLogic(iUserDalMock, iDisabledUserDalMock,
                mockedPublisher, iPendingLoginDalMock, new SecurityLogic(), new MockedJwtLogic().JwtLogic);
        }
    }
}