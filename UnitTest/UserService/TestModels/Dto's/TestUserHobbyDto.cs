using System;
using User_Service.Models;

namespace UnitTest.UserService.TestModels
{
    public class TestUserHobbyDto
    {
        public readonly UserHobbyDto UserHobby = new UserHobbyDto
        {
            Uuid = Guid.Parse("3fdec493-68d1-4100-8b89-8e8042f0aa59"),
            Hobby = "test",
            UserUuid = Guid.Parse("e058f548-3b18-4187-b4c2-3d10122f887c")
        };
    }
}