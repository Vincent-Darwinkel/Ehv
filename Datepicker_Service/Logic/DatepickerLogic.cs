using System;
using System.Threading.Tasks;
using Datepicker_Service.CustomExceptions;
using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.Models;
using Datepicker_Service.Models.FromFrontend;

namespace Datepicker_Service.Logic
{
    public class DatepickerLogic
    {
        private readonly IDatepickerDal _datepickerDal;

        public DatepickerLogic(IDatepickerDal datepickerDal)
        {
            _datepickerDal = datepickerDal;
        }

        /// <summary>
        /// Adds the datepicker to the database
        /// </summary>
        /// <param name="datepicker">The datepicker to add</param>
        public async Task Add(DatepickerDto datepicker)
        {
            await _datepickerDal.Add(datepicker);
        }

        /// <summary>
        /// Finds the datepicker by uuid
        /// </summary>
        /// <param name="uuid">The uuid to search for</param>
        /// <returns>The found datepicker, null if nothing found</returns>
        public async Task<DatepickerDto> Find(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                throw new UnprocessableException();
            }

            return await _datepickerDal.Find(uuid);
        }

        /// <summary>
        /// Updates the datepicker in the database
        /// </summary>
        /// <param name="datepicker">The updated datepicker</param>
        public async Task Update(DatepickerDto datepicker)
        {
            await _datepickerDal.Update(datepicker);
            // TODO inform subscribed users
        }

        /// <summary>
        /// Deletes the datepicker in the database by uuid
        /// </summary>
        /// <param name="uuid">The uuid to delete</param>
        public async Task Delete(Guid uuid)
        {
            await _datepickerDal.Delete(uuid);
            // TODO inform subscribed users
        }
    }
}