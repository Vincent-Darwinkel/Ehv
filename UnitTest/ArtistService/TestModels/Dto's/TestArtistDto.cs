using System;
using Favorite_Artist_Service.Model;

namespace UnitTest.ArtistService.TestModels
{
    public class TestArtistDto
    {
        public readonly FavoriteArtistDto Artist = new FavoriteArtistDto
        {
            Uuid = Guid.Parse("232127f6-4ca4-4a38-adb9-edc06b3567e5"),
            Name = "Test"
        };

        public readonly FavoriteArtistDto Empty = new FavoriteArtistDto
        {

        };
    }
}