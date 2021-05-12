using Authentication_Service.Dal.Interface;
using Moq;

namespace Authentication_Service.UnitTests.MockDals
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
