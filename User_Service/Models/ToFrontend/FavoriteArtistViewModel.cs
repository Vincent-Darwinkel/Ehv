using System;

namespace User_Service.Models.ToFrontend
{
    public class FavoriteArtistViewModel
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Artist { get; set; }
    }
}