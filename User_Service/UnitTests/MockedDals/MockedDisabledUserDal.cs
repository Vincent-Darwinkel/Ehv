using Moq;
using User_Service.Dal.Interfaces;

namespace User_Service.UnitTests.MockedDals
{
    public class MockedActivationDal
    {
        public readonly IActivationDal Mock;

        public MockedActivationDal()
        {
            var mock = new Mock<IActivationDal>();

            Mock = mock.Object;
        }
    }
}
