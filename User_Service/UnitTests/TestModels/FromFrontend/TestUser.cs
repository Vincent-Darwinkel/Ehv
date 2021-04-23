using System;
using System.Collections.Generic;
using User_Service.Enums;
using User_Service.Models.FromFrontend;

namespace User_Service.UnitTests.TestModels.FromFrontend
{
    public class TestUser
    {
        public readonly User User = new User
        {
            Username = "Test",
            About = "Test About",
            Email = "Test email",
            Gender = Gender.Male,
            AccountRole = AccountRole.User,
            BirthDate = new DateTime(2021, 05, 21),
            Hobbies = new List<UserHobby> { new TestUserHobby().UserHobby },
            FavoriteArtists = new List<FavoriteArtist> { new TestFavoriteArtist().FavoriteArtist }
        };
    }
}
