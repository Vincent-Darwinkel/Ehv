using System;
using System.Collections.Generic;
using User_Service.Enums;
using User_Service.Models;

namespace User_Service.UnitTests.TestModels
{
    public class TestUserDto
    {
        public UserDto User = new UserDto
        {
            Uuid = Guid.Parse("e058f548-3b18-4187-b4c2-3d10122f887c"),
            Username = "Test",
            About = "Test About",
            Email = "Test email",
            Gender = Gender.Male,
            AccountRole = AccountRole.User,
            BirthDate = new DateTime(2021, 05, 21),
            Hobbies = new List<UserHobbyDto> { new TestUserHobbyDto().UserHobby },
            FavoriteArtists = new List<FavoriteArtistDto> { new TestFavoriteArtistDto().FavoriteArtist }
        };

        public UserDto Admin = new UserDto
        {
            Uuid = Guid.Parse("e3f1f1d4-a2f9-4e24-b7ef-5c0fa7b039a8"),
            Username = "Test Admin",
            About = "Test About Admin",
            Email = "Test email Admin",
            Gender = Gender.Male,
            AccountRole = AccountRole.Admin,
            BirthDate = new DateTime(2009, 04, 10),
        };

        public UserDto SiteAdmin = new UserDto
        {
            Uuid = Guid.Parse("30910a5f-cede-41df-b245-8838a04abd35"),
            Username = "Test SiteAdmin",
            About = "Test About SiteAdmin",
            Email = "Test email SiteAdmin",
            Gender = Gender.Male,
            AccountRole = AccountRole.SiteAdmin,
            BirthDate = new DateTime(2011, 07, 1),
        };
    }
}