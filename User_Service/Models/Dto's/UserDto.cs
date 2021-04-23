using System;
using System.Collections.Generic;
using User_Service.Enums;

namespace User_Service.Models
{
    public class UserDto
    {
        public Guid Uuid { get; set; }
        public string Username { get; set; }
        public string About { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public AccountRole AccountRole { get; set; }
        public DateTime BirthDate { get; set; }
        public List<UserHobbyDto> Hobbies { get; set; }
        public List<FavoriteArtistDto> FavoriteArtists { get; set; }
    }
}