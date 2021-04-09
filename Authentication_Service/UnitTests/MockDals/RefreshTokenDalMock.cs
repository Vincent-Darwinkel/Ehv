using Authentication_Service.Dal.Interface;
using Authentication_Service.UnitTests.TestModels;
using Moq;

namespace Authentication_Service.UnitTests.MockDals
{
    public class RefreshTokenDalMock
    {
        public readonly IRefreshTokenDal Mock;

        public RefreshTokenDalMock()
        {
            var mock = new Mock<IRefreshTokenDal>();
            mock.Setup(m => m.Find(new TestRefreshTokenDto().RefreshTokenDto)).ReturnsAsync(new TestRefreshTokenDto().RefreshTokenDto);
            mock.Setup(m => m.Find(new TestRefreshTokenDto().RefreshTokenDtoWithoutExpirationDate)).ReturnsAsync(new TestRefreshTokenDto().RefreshTokenDto);
            mock.Setup(m => m.Find(new TestRefreshTokenDto().ExpiredToken)).ReturnsAsync(new TestRefreshTokenDto().ExpiredToken);

            Mock = mock.Object;
        }
    }
}