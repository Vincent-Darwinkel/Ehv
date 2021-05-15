using Event_Service.Models;
using System;
using System.Threading.Tasks;

namespace Event_Service.Dal.Interfaces
{
    public interface IEventStepDal
    {
        /// <summary>
        /// Adds the event step user to the database
        /// </summary>
        /// <param name="eventStepUser">The event step user to add</param>
        Task Add(EventStepDto eventStepUser);

        /// <summary>
        /// Find the step by uuid
        /// </summary>
        /// <param name="uuid">The uuid of the event step</param>
        /// <returns>The step that matches the uuid</returns>
        Task<EventStepDto> Find(Guid uuid);

        /// <summary>
        /// Update the specified step
        /// </summary>
        /// <param name="eventStep">The event step to update</param>
        Task Update(EventStepDto eventStep);

        /// <summary>
        /// Removes the step from the database
        /// </summary>
        /// <param name="eventStepUser"></param>
        Task Remove(EventStepDto eventStepUser);
    }
}
