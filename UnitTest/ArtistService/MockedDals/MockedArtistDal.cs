using Favorite_Artist_Service.Dal.Interfaces;
using Moq;

namespace UnitTest.ArtistService.MockedDals
{
    public class MockedArtistDal
    {
        public readonly IFavoriteArtistDal Mock;

        public MockedArtistDal()
        {
            var artistDal = new Mock<IFavoriteArtistDal>();
            Mock = artistDal.Object;
        }
    }
}