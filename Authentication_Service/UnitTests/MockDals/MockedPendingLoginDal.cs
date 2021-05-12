using Authentication_Service.Dal.Interface;
using Moq;

namespace Authentication_Service.UnitTests.MockDals
{
    public class MockedPendingLoginDal
    {
        public readonly IPendingLoginDal Mock;

        public MockedPendingLoginDal()
        {
            var mock = new Mock<IPendingLoginDal>();
            Mock = mock.Object;
        }
    }
}
