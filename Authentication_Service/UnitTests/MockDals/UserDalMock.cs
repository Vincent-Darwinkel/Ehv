using Authentication_Service.Dal.Interface;
using Authentication_Service.Models.Dto;
using Moq;

namespace Authentication_Service.UnitTests.MockDals
{
    public class UserDalMock
    {
        public readonly IUserDal Mock;

        public UserDalMock()
        {
            var testUser = new TestUserDto();
            var mock = new Mock<IUserDal>();
            mock.Setup(m => m.Find(testUser.Username)).ReturnsAsync(new UserDto());

            Mock = mock.Object;
        }
    }
}