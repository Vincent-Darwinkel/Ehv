using System;

namespace Authentication_Service.UnitTests
{
    public class TestUserDto
    {
        public Guid UserUuid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}