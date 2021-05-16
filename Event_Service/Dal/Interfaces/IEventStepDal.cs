using Event_Service.Models;
using System;
using System.Threading.Tasks;

namespace Event_Service.Dal.Interfaces
{
    public interface IEventStepDal
    {
        /// <summary>
        /// Find the step by uuid
        /// </summary>
        /// <param name="uuid">The uuid of the event step</param>
        /// <returns>The step that matches the uuid</returns>
        Task<EventStepDto> Find(Guid uuid);
    }
}
