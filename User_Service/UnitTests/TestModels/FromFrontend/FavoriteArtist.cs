using System;
using User_Service.Models.FromFrontend;

namespace User_Service.UnitTests.TestModels.FromFrontend
{
    public class TestFavoriteArtist
    {
        public readonly FavoriteArtist FavoriteArtist = new FavoriteArtist
        {
            Uuid = Guid.Parse("ecf9f055-726d-4793-892d-da84fd634adc"),
            Artist = "Test",
            UserUuid = Guid.Parse("e058f548-3b18-4187-b4c2-3d10122f887c")
        };
    }
}
