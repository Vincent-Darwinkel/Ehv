using System;
using System.Collections.Generic;
using User_Service.Enums;
using User_Service.Models.FromFrontend;

namespace User_Service.Models.ToFrontend
{
    public class UserViewModel
    {
        public Guid Uuid { get; set; }
        public string Username { get; set; }
        public string About { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public AccountRole AccountRole { get; set; }
        public DateTime BirthDate { get; set; }
        public List<UserHobby> Hobbies { get; set; }
        public List<FavoriteArtist> FavoriteArtists { get; set; }
    }
}