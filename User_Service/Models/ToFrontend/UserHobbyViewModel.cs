using System;

namespace User_Service.Models.ToFrontend
{
    public class UserHobbyViewModel
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Hobby { get; set; }
    }
}