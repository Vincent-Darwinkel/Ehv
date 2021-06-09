using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Event_Service.Models.HelperFiles;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Event_Service.Logic
{
    public class EventDateUserLogic
    {
        private readonly IEventDateUserDal _eventDateUserDal;

        public EventDateUserLogic(IEventDateUserDal eventDateUserDal)
        {
            _eventDateUserDal = eventDateUserDal;
        }

        /// <summary>
        /// Unsubscribe the requestingUser from an event date
        /// </summary>
        /// <param name="eventDateUuid"></param>
        /// <param name="requestingUser"></param>
        /// <returns></returns>
        public async Task Remove(Guid eventDateUuid, UserHelper requestingUser)
        {
            EventDateUserDto dbEventDateUser = await _eventDateUserDal.Find(eventDateUuid, requestingUser.Uuid);
            if (dbEventDateUser == null)
            {
                throw new NoNullAllowedException(nameof(dbEventDateUser));
            }

            await _eventDateUserDal.Remove(dbEventDateUser);
        }

        public async Task DeleteUserFromEventDate(Guid userUuid)
        {
            await _eventDateUserDal.Remove(userUuid);
        }
    }
}