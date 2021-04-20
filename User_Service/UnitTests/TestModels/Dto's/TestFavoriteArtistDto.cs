using System;
using User_Service.Models;

namespace User_Service.UnitTests.TestModels
{
    public class TestFavoriteArtistDto
    {
        public readonly FavoriteArtistDto FavoriteArtist = new FavoriteArtistDto
        {
            Uuid = Guid.Parse("ecf9f055-726d-4793-892d-da84fd634adc"),
            Artist = "Test",
            UserUuid = Guid.Parse("e058f548-3b18-4187-b4c2-3d10122f887c")
        };
    }
}
