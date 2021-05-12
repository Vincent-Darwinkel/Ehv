using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Favorite_Artist_Service.CustomExceptions;
using Favorite_Artist_Service.Dal.Interfaces;
using Favorite_Artist_Service.Model;

namespace Favorite_Artist_Service.Logic
{
    public class FavoriteArtistLogic
    {
        private readonly IFavoriteArtistDal _favoriteArtistDal;

        public FavoriteArtistLogic(IFavoriteArtistDal favoriteArtistDal)
        {
            _favoriteArtistDal = favoriteArtistDal;
        }

        public async Task Add(FavoriteArtistDto hobby)
        {
            await _favoriteArtistDal.Add(hobby);
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

        public async Task Update(FavoriteArtistDto hobby)
        {
            if (hobby.Uuid == Guid.Empty || string.IsNullOrEmpty(hobby.Name))
            {
                throw new UnprocessableException();
            }

            await _favoriteArtistDal.Update(hobby);
        }

        public async Task Delete(Guid uuid)
        {
            await _favoriteArtistDal.Delete(uuid);
        }
    }
}
