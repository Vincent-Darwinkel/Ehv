using Authentication_Service.Enums;
using Authentication_Service.Models.Dto;
using System;

namespace UnitTest.AuthenticationService.TestModels
{
    public class TestUserDto
    {
        public UserDto User = new UserDto
        {
            Uuid = Guid.Parse("e058f548-3b18-4187-b4c2-3d10122f887c"),
            Password = "$argon2id$v=19$m=16,t=2,p=1$WWlrcmZsWEh5ekROT3dmbA$1TWLto1ByQbW15fYbjZRRw", // password = test
            Username = "Test",
            AccountRole = AccountRole.User
        };

        public UserDto SiteAdmin = new UserDto
        {
            Uuid = Guid.Parse("92597ff9-3ba3-4814-a5ed-679b5a1b0c38"),
            Password = "$argon2id$v=19$m=16,t=2,p=1$WWlrcmZsWEh5ekROT3dmbA$1TWLto1ByQbW15fYbjZRRw", // password = test
            Username = "SiteAdmin",
            AccountRole = AccountRole.SiteAdmin
        };
    }
}
