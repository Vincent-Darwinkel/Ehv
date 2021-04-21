using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Event_Service.Models.FromFrontend;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Event_Service
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
    }
}
