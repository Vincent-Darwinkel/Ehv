using System;

namespace User_Service.Models
{
    public class FavoriteArtistDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Artist { get; set; }
    }
}
