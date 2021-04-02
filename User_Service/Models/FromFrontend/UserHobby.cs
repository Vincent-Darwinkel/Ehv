using System;

namespace User_Service.Models.FromFrontend
{
    public class UserHobby
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public Guid UserUuid { get; set; }
        public Guid HobbyUuid { get; set; }
    }
}
