using Event_Service.Models;
using System;
using System.Threading.Tasks;

namespace Event_Service.Dal.Interfaces
{
    public interface IEventStepUserDal
    {
        /// <summary>
        /// Saves the event step user in the database
        /// </summary>
        /// <param name="eventStepUser">The event step user to add</param>
        public Task Add(EventStepUserDto eventStepUser);

        /// <summary>
        /// Finds the even step user in the database which matches the specified uuids
        /// </summary>
        /// <param name="eventStepUuid">The uuid of the event step</param>
        /// <param name="userUuid">The uuid of the user</param>
        /// <returns>The found event step user</returns>
        public Task<EventStepUserDto> Find(Guid eventStepUuid, Guid userUuid);

        /// <summary>
        /// Removes the event step user which matches the uuid
        /// </summary>
        /// <param name="eventStepUser">The event step user to remove</param>
        public Task Remove(EventStepUserDto eventStepUser);

        /// <summary>
        /// Removes the user from the event step
        /// </summary>
        /// <param name="userUuid">The uuid of the user</param>
        public Task Remove(Guid userUuid);
    }
}