using System;
using System.Threading.Tasks;
using Datepicker_Service.Models;

namespace Datepicker_Service.Dal.Interfaces
{
    public interface IDatepickerDal
    {
        /// <summary>
        /// Adds the datepicker to the database
        /// </summary>
        /// <param name="datepicker">The datepicker to add</param>
        Task Add(DatepickerDto datepicker);

        /// <summary>
        /// Finds the datepicker by uuid
        /// </summary>
        /// <param name="uuid">The uuid to search for</param>
        /// <returns>The found datepicker, null if nothing found</returns>
        Task<DatepickerDto> Find(Guid uuid);

        /// <summary>
        /// Checks if the datepicker exists
        /// </summary>
        /// <param name="title">The title to search for</param>
        /// <returns>True if datepicker with title exists, false if does not exists</returns>
        Task<bool> Exists(string title);

        /// <summary>
        /// Updates the datepicker
        /// </summary>
        /// <param name="datepicker">The updated datepicker</param>
        Task Update(DatepickerDto datepicker);

        /// <summary>
        /// Deletes the datepicker by uuid
        /// </summary>
        /// <param name="uuid">The uuid to delete</param>
        Task Delete(Guid uuid);
    }
}