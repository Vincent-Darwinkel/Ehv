using System;
using System.Threading.Tasks;
using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Event_Service.Models.HelperFiles;

namespace Event_Service.Logic
{
    public class EventDateLogic
    {
        private readonly IEventDateDal _eventDateDal;
        private readonly IEventDal _eventDal;

        public EventDateLogic(IEventDateDal eventDateDal, IEventDal eventDal)
        {
            _eventDateDal = eventDateDal;
            _eventDal = eventDal;
        }

        public async Task Remove(Guid eventDateUuid, UserHelper requestingUser)
        {
            EventDateDto dbEventDate = await _eventDateDal.Find(eventDateUuid);
            EventDto dbEvent = await _eventDal.Find(dbEventDate.EventUuid);
            if (dbEvent.AuthorUuid != requestingUser.Uuid)
            {
                throw new UnauthorizedAccessException();
            }

            await _eventDateDal.Remove(dbEventDate);
        }
    }
}
