using System;
using System.Collections.Generic;
using User_Service.Enums;
using User_Service.Models;
using User_Service.Models.FromFrontend;

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
            FavoriteArtists = new List<FavoriteArtistDto> { new TestfavoriteArtist().FavoriteArtist }
        };

        public UserDto SiteAdmin = new UserDto
        {
            Uuid = Guid.Parse("30910a5f-cede-41df-b245-8838a04abd35"),
            Username = "Test SiteAdmin",
            About = "Test About SiteAdmin",
            Email = "Test email SiteAdmin",
            Gender = Gender.Male,
            AccountRole = AccountRole.User,
            BirthDate = new DateTime(2011, 07, 1),
        };
    }
}