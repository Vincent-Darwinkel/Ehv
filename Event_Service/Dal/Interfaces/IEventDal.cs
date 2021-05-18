using Event_Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Event_Service.Dal.Interfaces
{
    public interface IEventDal
    {
        /// <summary>
        /// Adds the event to the database
        /// </summary>
        /// <param name="eventDto"></param>
        public Task Add(EventDto eventDto);

        /// <returns>All events</returns>
        public Task<List<EventDto>> All();

        /// <summary>
        /// Checks if the event exists
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns>True if found, false if not found</returns>
        public Task<bool> Exists(string eventName);

        /// <summary>
        /// Find event by uuid
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns>The found event, null if nothing is found</returns>
        public Task<EventDto> Find(Guid uuid);

        /// <summary>
        /// Updates the specified event
        /// </summary>
        /// <param name="eventToUpdate"></param>
        public Task Update(EventDto eventToUpdate);

        /// <summary>
        /// Removes the event from the database
        /// </summary>
        /// <param name="eventToRemove"></param>
        public Task Delete(EventDto eventToRemove);
    }
}