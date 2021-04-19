using System;

namespace User_Service.Models.FromFrontend
{
    public class UserHobby
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Hobby { get; set; }
    }
}
