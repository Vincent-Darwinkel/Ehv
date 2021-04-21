using System;
using System.Data;
using System.Threading.Tasks;
using Datepicker_Service.CustomExceptions;
using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.Models;
using Datepicker_Service.Models.FromFrontend;
using Datepicker_Service.Models.HelperFiles;
using Datepicker_Service.RabbitMq.Rpc;
using RabbitMQ.Client;

namespace Datepicker_Service.Logic
{
    public class DatepickerLogic
    {
        private readonly IDatepickerDal _datepickerDal;
        private readonly IModel _model;

        public DatepickerLogic(IDatepickerDal datepickerDal, IModel model)
        {
            _datepickerDal = datepickerDal;
            _model = model;
        }

        /// <summary>
        /// Adds the datepicker to the database
        /// </summary>
        /// <param name="datepicker">The datepicker to add</param>
        /// <param name="requestingUser">The user that made the request</param>
        public async Task Add(DatepickerDto datepicker, User requestingUser)
        {
            if (datepicker?.Dates == null || datepicker.Dates.Count <= 0)
            {
                throw new ArgumentNullException(nameof(datepicker));
            }

            datepicker.AuthorUuid = requestingUser.Uuid;
            datepicker.Dates.ForEach(d => d.DatePickerUuid = datepicker.Uuid);
            var rpcClient = new RpcClient(_model);

            bool datepickerExists = await _datepickerDal.Exists(datepicker.Title);
            bool eventExists = rpcClient.Call<bool>(datepicker.Title, RabbitMqRouting.EventExists);
            if (eventExists || datepickerExists)
            {
                throw new DuplicateNameException();
            }

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