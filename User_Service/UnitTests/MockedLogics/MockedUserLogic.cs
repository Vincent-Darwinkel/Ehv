using AutoMapper;
using Moq;
using User_Service.Logic;
using User_Service.Models.HelperFiles;
using User_Service.RabbitMq.Publishers;
using User_Service.UnitTests.MockedDals;

namespace User_Service.UnitTests.MockedLogics
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
            UserLogic = new UserLogic(mockedUserDal, mockedDisabledUserDal, mockedActivationDal, null, mockedProducer.Object);
        }
    }
}
