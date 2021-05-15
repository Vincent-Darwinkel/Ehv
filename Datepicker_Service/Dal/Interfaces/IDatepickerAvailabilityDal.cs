using Datepicker_Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Datepicker_Service.Dal.Interfaces
{
    public interface IDatepickerAvailabilityDal
    {
        /// <summary>
        /// Adds the availabilities in the database
        /// </summary>
        /// <param name="availabilities"></param>
        public Task Add(List<DatepickerAvailabilityDto> availabilities);

        /// <summary>
        /// Finds the availabilities that match the uuid in the collection
        /// </summary>
        /// <param name="dateUuidCollection">The uuid of the dates</param>
        /// <param name="userUuid">The uuid of the user</param>
        /// <returns>The found availabilities</returns>
        public Task<List<DatepickerAvailabilityDto>> Find(List<Guid> dateUuidCollection, Guid userUuid);

        public Task<DatepickerAvailabilityDto> Find(DatepickerAvailabilityDto availability);

        /// <summary>
        /// Removes the availabilities in the database by date uuid and user uuid
        /// </summary>
        /// <param name="dateUuidCollection">The uuid of the availabilities to remove</param>
        /// <param name="userUuid">The uuid of the user</param>
        public Task Delete(IEnumerable<Guid> dateUuidCollection, Guid userUuid);
    }
}