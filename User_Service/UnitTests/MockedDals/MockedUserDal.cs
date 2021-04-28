﻿using System;
using System.Collections.Generic;
using Moq;
using User_Service.Dal;
using User_Service.Models;
using User_Service.UnitTests.TestModels;

namespace User_Service.UnitTests.MockedDals
{
    public class MockedUserDal
    {
        public readonly IUserDal Mock;

        public MockedUserDal()
        {
            var testUser = new TestUserDto().User;
            var mock = new Mock<IUserDal>();
            mock.Setup(m => m.All()).ReturnsAsync(new List<UserDto> { testUser });
            mock.Setup(m => m.Exists(testUser.Username, testUser.Email)).ReturnsAsync(true);
            mock.Setup(m => m.Exists("test", testUser.Email)).ReturnsAsync(true);
            mock.Setup(m => m.Find(testUser.Username, testUser.Email)).ReturnsAsync(testUser);
            mock.Setup(m => m.Find(testUser.Uuid)).ReturnsAsync(testUser);
            mock.Setup(m => m.Find(new List<Guid> { testUser.Uuid })).ReturnsAsync(new List<UserDto> { testUser });
            mock.Setup(m => m.Find(new TestUserDto().SiteAdmin.Uuid)).ReturnsAsync(new TestUserDto().SiteAdmin);

            Mock = mock.Object;
        }
    }
}