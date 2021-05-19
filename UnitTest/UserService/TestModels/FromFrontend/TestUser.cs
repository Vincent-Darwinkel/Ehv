using System;
using System.Collections.Generic;
using User_Service.Enums;
using User_Service.Models.FromFrontend;

namespace UnitTest.UserService.TestModels.FromFrontend
{
    public class TestUser
    {
        public readonly User NewUser = new User
        {
            Username = "New User",
            About = "Test About",
            Email = "testnewuser@example.com",
            Gender = Gender.Male,
            Password = "f0j0923j023f",
            AccountRole = AccountRole.User,
            BirthDate = new DateTime(2021, 01, 6),
            Hobbies = new List<UserHobby> { new TestUserHobby().UserHobby },
            FavoriteArtists = new List<FavoriteArtist> { new TestFavoriteArtist().FavoriteArtist }
        };

        public readonly User User = new User
        {
            Username = "Test",
            About = "Test About",
            Email = "testuser@example.com",
            Gender = Gender.Male,
            Password = "f0j0923j023f",
            AccountRole = AccountRole.User,
            BirthDate = new DateTime(2021, 05, 21),
            Hobbies = new List<UserHobby> { new TestUserHobby().UserHobby },
            FavoriteArtists = new List<FavoriteArtist> { new TestFavoriteArtist().FavoriteArtist }
        };

        public readonly User Admin = new User
        {
            Username = "Test Admin",
            About = "Test About",
            Email = "testadmin@example.com",
            Gender = Gender.Male,
            Password = "f0j0923j023f",
            AccountRole = AccountRole.Admin,
            BirthDate = new DateTime(2021, 05, 21),
            Hobbies = new List<UserHobby> { new TestUserHobby().UserHobby },
            FavoriteArtists = new List<FavoriteArtist> { new TestFavoriteArtist().FavoriteArtist }
        };
    }
}
