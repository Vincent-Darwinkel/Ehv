using System;

namespace Authentication_Service.Models.Dto
{
    public class PendingLoginDto
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public Guid UserUuid { get; set; }
        public int AccessCode { get; set; } = new Random().Next(100000, 1000000);
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddMinutes(1);
    }
}