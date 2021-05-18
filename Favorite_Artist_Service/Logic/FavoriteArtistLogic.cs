using Favorite_Artist_Service.CustomExceptions;
using Favorite_Artist_Service.Dal.Interfaces;
using Favorite_Artist_Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Favorite_Artist_Service.Logic
{
    public class FavoriteArtistLogic
    {
        private readonly IFavoriteArtistDal _favoriteArtistDal;

        public FavoriteArtistLogic(IFavoriteArtistDal favoriteArtistDal)
        {
            _favoriteArtistDal = favoriteArtistDal;
        }

        public async Task Add(FavoriteArtistDto artist)
        {
            if (string.IsNullOrEmpty(artist.Name))
            {
                throw new UnprocessableException();
            }

            await _favoriteArtistDal.Add(artist);
        }

        public async Task<List<FavoriteArtistDto>> All()
        {
            return await _favoriteArtistDal.All();
        }

        public async Task<string> AllRabbitMq()
        {
            List<FavoriteArtistDto> result = await All();
            return await Task.Factory
                .StartNew(() => Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }

        public async Task Update(FavoriteArtistDto artist)
        {
            if (artist.Uuid == Guid.Empty || string.IsNullOrEmpty(artist.Name))
            {
                throw new UnprocessableException();
            }

            await _favoriteArtistDal.Update(artist);
        }

        public async Task Delete(List<Guid> uuidCollection)
        {
            if (!uuidCollection.Any())
            {
                throw new UnprocessableException();
            }

            await _favoriteArtistDal.Delete(uuidCollection);
        }
    }
}
