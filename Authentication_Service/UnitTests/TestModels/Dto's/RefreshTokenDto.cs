using System;
using Authentication_Service.Models.Dto;

namespace Authentication_Service.UnitTests.TestModels
{
    public class TestRefreshTokenDto
    {
        public RefreshTokenDto RefreshTokenDtoWithoutExpirationDate = new RefreshTokenDto
        {
            UserUuid = new TestUserDto().User.Uuid,
            RefreshToken = "h2398jfsoafiejwoaiefjiogearjeagrijeagrieagr"
        };
        public RefreshTokenDto RefreshTokenDto = new RefreshTokenDto
        {
            UserUuid = new TestUserDto().User.Uuid,
            ExpirationDate = DateTime.Now.AddDays(2),
            RefreshToken = "12eds4y545egg44qw5g45g"
        };
        public RefreshTokenDto ExpiredToken = new RefreshTokenDto
        {
            UserUuid = new TestUserDto().User.Uuid,
            ExpirationDate = DateTime.Now,
            RefreshToken = "123124werfwefweaf"
        };
    }
}