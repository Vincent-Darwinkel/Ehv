using Favorite_Artist_Service.Logic;
using UnitTest.ArtistService.MockedDals;

namespace UnitTest.ArtistService.MockedLogics
{
    public class MockedArtistLogic
    {
        public readonly FavoriteArtistLogic ArtistLogic;

        public MockedArtistLogic()
        {
            var artistDal = new MockedArtistDal().Mock;
            var artistLogic = new FavoriteArtistLogic(artistDal);
            ArtistLogic = artistLogic;
        }
    }
}