using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Datepicker_Service.Models;

namespace Datepicker_Service.Dal.Interfaces
{
    public interface IDatepickerDateDal
    {
        /// <summary>
        /// Add the specified dates
        /// </summary>
        /// <param name="dates">The dates to add</param>
        Task Add(List<DatepickerDateDto> dates);

        /// <summary>
        /// Finds which match the datepicker uuid
        /// </summary>
        /// <param name="datepickerUuid">The uuid of the date picker</param>
        /// <returns>The find dates</returns>
        Task<List<DatepickerDateDto>> Find(Guid datepickerUuid);

        /// <summary>
        /// Deletes the specified dates from the database
        /// </summary>
        /// <param name="dates">The dates to delete</param>
        Task Delete(List<DatepickerDateDto> dates);
    }
}
