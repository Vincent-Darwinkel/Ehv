using Authentication_Service.Dal.Interface;
using Authentication_Service.UnitTests.TestModels;
using Moq;

namespace Authentication_Service.UnitTests.MockDals
{
    public class MockedDisabledUserDal
    {
        public readonly IDisabledUserDal Mock;

        public MockedDisabledUserDal()
        {
            var testUser = new TestUserDto().User;
            var mock = new Mock<IDisabledUserDal>();

            Mock = mock.Object;
        }
    }
}
