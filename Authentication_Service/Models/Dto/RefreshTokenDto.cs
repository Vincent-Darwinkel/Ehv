using System;

namespace Authentication_Service.Models.Dto
{
    public class RefreshTokenDto
    {
        public Guid UserUuid { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
