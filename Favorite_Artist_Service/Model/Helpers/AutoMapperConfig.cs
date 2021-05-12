using AutoMapper;
using Favorite_Artist_Service.Model.FromFrontend;
using Favorite_Artist_Service.Model.ToFrontend;

namespace Favorite_Artist_Service.Model.Helpers
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FavoriteArtistDto, FavoriteArtistViewmodel>();
                cfg.CreateMap<FavoriteArtist, FavoriteArtistDto>();
            });
    }
}
