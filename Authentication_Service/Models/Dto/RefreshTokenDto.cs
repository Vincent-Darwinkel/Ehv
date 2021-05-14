using System;

namespace Authentication_Service.Models.Dto
{
    public class RefreshTokenDto
    {
        public Guid UserUuid { get; set; }
        public Guid RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
