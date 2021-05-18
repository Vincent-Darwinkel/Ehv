using Authentication_Service.Logic;
using Authentication_Service.Models.HelperFiles;
using Authentication_Service.Models.RabbitMq;
using Authentication_Service.RabbitMq.Publishers;
using Authentication_Service.RabbitMq.Rpc;
using Moq;
using System;
using System.Collections.Generic;
using UnitTest.AuthenticationService.MockDals;
using UnitTest.AuthenticationService.TestModels;
using UnitTest.AuthenticationService.TestModels.RabbitMq;
using MockedUserDal = UnitTest.AuthenticationService.MockDals.MockedUserDal;

namespace UnitTest.AuthenticationService.MockedLogics
{
    public class MockedAuthenticationLogic
    {
        public readonly AuthenticationLogic AuthenticationLogic;

        public MockedAuthenticationLogic()
        {
            var siteAdminUser = new TestUserDto().SiteAdmin;
            var iUserDalMock = new MockedUserDal().Mock;
            var iPendingLoginDalMock = new MockedPendingLoginDal().Mock;
            var mockedPublisher = new Mock<IPublisher>().Object;
            var mockedRpcClient = new Mock<IRpcClient>();

            mockedRpcClient.Setup(rpc => rpc.Call<List<UserRabbitMqSensitiveInformation>>
                (new List<Guid> { siteAdminUser.Uuid }, RabbitMqQueues.FindUserQueue))
                .Returns(new List<UserRabbitMqSensitiveInformation> { new TestRabbitMqUserSensitiveInformation().SiteAdminFromUserService });

            AuthenticationLogic = new AuthenticationLogic(iUserDalMock, mockedPublisher, iPendingLoginDalMock,
                new SecurityLogic(), new MockedJwtLogic().JwtLogic, mockedRpcClient.Object);
        }
    }
}