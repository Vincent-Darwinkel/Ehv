using System;

namespace User_Service.Models.FromFrontend
{
    public class FavoriteArtist
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Artist { get; set; }
    }
}