using Authentication_Service.Enums;
using System;

namespace Authentication_Service.Models.Dto
{
    public class UserDto
    {
        public Guid Uuid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AccountRole AccountRole { get; set; }
    }
}