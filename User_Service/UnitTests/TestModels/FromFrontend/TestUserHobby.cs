﻿using System;
using User_Service.Models.FromFrontend;

namespace User_Service.UnitTests.TestModels.FromFrontend
{
    public class TestUserHobby
    {
        public readonly UserHobby UserHobby = new UserHobby
        {
            Uuid = Guid.Parse("3fdec493-68d1-4100-8b89-8e8042f0aa59"),
            Hobby = "test",
            UserUuid = Guid.Parse("e058f548-3b18-4187-b4c2-3d10122f887c")
        };
    }
}