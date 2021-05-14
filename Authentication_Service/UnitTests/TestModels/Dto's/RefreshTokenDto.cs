using System;
using Authentication_Service.Models.Dto;

namespace Authentication_Service.UnitTests.TestModels
{
    public class TestRefreshTokenDto
    {
        public RefreshTokenDto RefreshTokenDtoWithoutExpirationDate = new RefreshTokenDto
        {
            UserUuid = new TestUserDto().User.Uuid,
            RefreshToken = Guid.Parse("a7b1d068-e70d-4ad6-b452-b55e84cc4bd9")
        };
        public RefreshTokenDto RefreshTokenDto = new RefreshTokenDto
        {
            UserUuid = new TestUserDto().User.Uuid,
            ExpirationDate = DateTime.Now.AddDays(2),
            RefreshToken = Guid.Parse("5034aacd-df4f-4e03-ab48-0d3c078db22d")
        };
        public RefreshTokenDto ExpiredToken = new RefreshTokenDto
        {
            UserUuid = new TestUserDto().User.Uuid,
            ExpirationDate = DateTime.Now,
            RefreshToken = Guid.Parse("35ec70c2-3f54-4b9f-baeb-f322f2822cfd")
        };
    }
}