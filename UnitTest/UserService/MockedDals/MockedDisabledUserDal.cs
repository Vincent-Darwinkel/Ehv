using Moq;
using UnitTest.UserService.TestModels;
using User_Service.Dal.Interfaces;

namespace UnitTest.UserService.MockedDals
{
    public class MockedActivationDal
    {
        public readonly IActivationDal Mock;

        public MockedActivationDal()
        {
            var mock = new Mock<IActivationDal>();
            var activationDto = new TestActivationDto().Activation;
            mock.Setup(ad => ad.Find(activationDto.Code)).ReturnsAsync(activationDto);
            Mock = mock.Object;
        }
    }
}
