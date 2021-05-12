using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Favorite_Artist_Service.Logic;
using Favorite_Artist_Service.Model;
using Favorite_Artist_Service.Model.FromFrontend;
using Favorite_Artist_Service.Model.ToFrontend;

namespace Favorite_Artist_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteArtistController : ControllerBase
    {
        private readonly FavoriteArtistLogic _favoriteArtistLogic;
        private readonly LogLogic _logLogic;
        private readonly IMapper _mapper;

        public FavoriteArtistController(FavoriteArtistLogic favoriteArtistLogic, LogLogic logLogic, IMapper mapper)
        {
            _favoriteArtistLogic = favoriteArtistLogic;
            _logLogic = logLogic;
            _mapper = mapper;
        }

        public async Task<ActionResult> Add(FavoriteArtist favoriteArtist)
        {
            try
            {
                var favoriteArtistToAdd = _mapper.Map<FavoriteArtistDto>(favoriteArtist);
                favoriteArtistToAdd.Uuid = Guid.NewGuid();

                await _favoriteArtistLogic.Add(favoriteArtistToAdd);
                return Ok();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<List<FavoriteArtistViewmodel>>> All()
        {
            try
            {
                List<FavoriteArtistDto> favoriteArtists = await _favoriteArtistLogic.All();
                return _mapper.Map<List<FavoriteArtistViewmodel>>(favoriteArtists);
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult> Update(FavoriteArtist favoriteArtist)
        {
            try
            {
                var favoriteArtistToUpdate = _mapper.Map<FavoriteArtistDto>(favoriteArtist);
                await _favoriteArtistLogic.Update(favoriteArtistToUpdate);
                return Ok();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult> Delete(Guid uuid)
        {
            try
            {
                await _favoriteArtistLogic.Delete(uuid);
                return Ok();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
