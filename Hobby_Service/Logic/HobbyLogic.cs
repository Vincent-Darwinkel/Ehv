using Hobby_Service.CustomExceptions;
using Hobby_Service.Dal.Interfaces;
using Hobby_Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hobby_Service.Logic
{
    public class HobbyLogic
    {
        private readonly IHobbyDal _hobbyDal;

        public HobbyLogic(IHobbyDal hobbyDal)
        {
            _hobbyDal = hobbyDal;
        }

        public async Task Add(HobbyDto hobby)
        {
            await _hobbyDal.Add(hobby);
        }

        public async Task<List<HobbyDto>> All()
        {
            return await _hobbyDal.All();
        }

        public async Task<string> AllRabbitMq()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(await All());
        }

        public async Task Update(HobbyDto hobby)
        {
            if (hobby.Uuid == Guid.Empty || string.IsNullOrEmpty(hobby.Name))
            {
                throw new UnprocessableException();
            }

            await _hobbyDal.Update(hobby);
        }

        public async Task Delete(Guid uuid)
        {
            await _hobbyDal.Delete(uuid);
        }
    }
}