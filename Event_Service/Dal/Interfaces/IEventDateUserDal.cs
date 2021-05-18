using Event_Service.Models;
using System;
using System.Threading.Tasks;

namespace Event_Service.Dal.Interfaces
{
    public interface IEventDateUserDal
    {
        /// <summary>
        /// Finds the event date user which matches the event date uuid and user uuid
        /// </summary>
        /// <param name="eventDateUuid"></param>
        /// <param name="userUuid"></param>
        /// <returns>The found event date user</returns>
        public Task<EventDateUserDto> Find(Guid eventDateUuid, Guid userUuid);

        /// <summary>
        /// Removes the specified event date user
        /// </summary>
        /// <param name="eventDateUserToRemove"></param>
        public Task Remove(EventDateUserDto eventDateUserToRemove);
    }
}
