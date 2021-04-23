using System;
using System.Threading.Tasks;
using Event_Service.Models;

namespace Event_Service.Dal.Interfaces
{
    public interface IEventDal
    {
        /// <summary>
        /// Adds the event to the database
        /// </summary>
        /// <param name="eventToAdd">The event to add</param>
        Task Add(EventDto eventToAdd);

        /// <summary>
        /// Finds the event by uuid
        /// </summary>
        /// <param name="uuid">The uuid to search for</param>
        /// <returns>The found event, null if nothing found</returns>
        Task<EventDto> Find(Guid uuid);

        /// <summary>
        /// Checks if the event exists
        /// </summary>
        /// <param name="title">The title to search for</param>
        /// <returns>True if event with title exists, false if does not exists</returns>
        Task<bool> Exists(string title);

        /// <summary>
        /// Updates the event
        /// </summary>
        /// <param name="eventToUpdate">The updated event</param>
        Task Update(EventDto eventToUpdate);

        /// <summary>
        /// Deletes the event by uuid
        /// </summary>
        /// <param name="uuid">The uuid to delete</param>
        Task Delete(Guid uuid);
    }
}