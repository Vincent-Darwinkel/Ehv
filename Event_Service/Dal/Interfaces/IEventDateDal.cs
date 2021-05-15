using Event_Service.Models;
using System;
using System.Threading.Tasks;

namespace Event_Service.Dal.Interfaces
{
    public interface IEventDateDal
    {
        /// <summary>
        /// Finds the matching event date by uuid
        /// </summary>
        /// <param name="eventDateUuid"></param>
        /// <returns>The event date with matching uuid</returns>
        Task<EventDateDto> Find(Guid eventDateUuid);

        /// <summary>
        /// Removes the specified event date
        /// </summary>
        /// <param name="eventDateToRemove"></param>
        Task Remove(EventDateDto eventDateToRemove);
    }
}