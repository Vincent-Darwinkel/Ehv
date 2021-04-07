using System;

namespace User_Service.Models
{
    public class UserHobbyDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Hobby { get; set; }
    }
}
