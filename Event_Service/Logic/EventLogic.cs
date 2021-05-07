using AutoMapper;
using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Event_Service.Models.FromFrontend;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Service.Logic
{
    public class EventLogic
    {
        private readonly IEventDal _eventDal;
        private readonly IMapper _mapper;

        public EventLogic(IEventDal eventDal, IMapper mapper)
        {
            _eventDal = eventDal;
            _mapper = mapper;
        }

        public async Task Add(Event eventToAdd)
        {
            if (await _eventDal.Exists(eventToAdd.Title))
            {
                throw new DuplicateNameException();
            }

            var eventDto = _mapper.Map<EventDto>(eventToAdd);
            await _eventDal.Add(eventDto);

            List<Guid> usersToNotifyUuidCollection = eventDto.EventDates
                .SelectMany(e => e.EventDateUsers
                    .Select(u => u.UserUuid))
                .Distinct()
                .ToList();

            if (usersToNotifyUuidCollection.Count > 0)
            {

            }
        }

        /// <summary>
        /// Checks if event with same name exists, this method is called by the Rpc server
        /// </summary>
        /// <param name="title">The title to search for</param>
        /// <returns>True or false in string format</returns>
        public async Task<string> Exists(string title)
        {
            bool exists = await _eventDal.Exists(title);
            return Newtonsoft.Json.JsonConvert.SerializeObject(exists);
        }
    }
}
