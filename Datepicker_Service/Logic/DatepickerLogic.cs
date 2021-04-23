using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Datepicker_Service.CustomExceptions;
using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.Models;
using Datepicker_Service.Models.FromFrontend;
using Datepicker_Service.Models.HelperFiles;
using Datepicker_Service.Models.RabbitMq;
using Datepicker_Service.RabbitMq.Publishers;
using Datepicker_Service.RabbitMq.Rpc;
using RabbitMQ.Client;
using User_Service.Models.HelperFiles;

namespace Datepicker_Service.Logic
{
    public class DatepickerLogic
    {
        private readonly IDatepickerDal _datepickerDal;
        private readonly IModel _model;
        private readonly IPublisher _publisher;

        public DatepickerLogic(IDatepickerDal datepickerDal, IModel model, IPublisher publisher)
        {
            _datepickerDal = datepickerDal;
            _model = model;
            _publisher = publisher;
        }

        private bool DatepickerValid(DatepickerDto datepicker)
        {
            return !string.IsNullOrEmpty(datepicker.Title) &&
                   !string.IsNullOrEmpty(datepicker.Description) &&
                   !string.IsNullOrEmpty(datepicker.Location) &&
                   datepicker.AuthorUuid != Guid.Empty &&
                   datepicker.Dates.Any() &&
                   datepicker.Expires > DateTime.Now &&
                   datepicker.Uuid != Guid.Empty;
        }

        /// <summary>
        /// Adds the datepicker to the database
        /// </summary>
        /// <param name="datepicker">The datepicker to add</param>
        /// <param name="requestingUser">The user that made the request</param>
        public async Task Add(DatepickerDto datepicker, User requestingUser)
        {
            if (!DatepickerValid(datepicker))
            {
                throw new UnprocessableException(nameof(datepicker));
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
            if (!DatepickerValid(datepicker))
            {
                throw new UnprocessableException(nameof(datepicker));
            }

            DatepickerDto dbDatepicker = await _datepickerDal.Find(datepicker.Uuid);
            await _datepickerDal.Update(datepicker);
            List<Guid> userUuidCollection = dbDatepicker.Dates.SelectMany(d => d.UserAvailabilities.Select(ua => ua.UserUuid))
                .ToList();

            if (!userUuidCollection.Any())
            {
                return;
            }

            // Inform users about the update
            var rpcClient = new RpcClient(_model);
            var users = rpcClient.Call<List<UserRabbitMq>>(userUuidCollection, RabbitMqRouting.FindUser);

            var emails = users
                .Select(user => new EmailRabbitMq
                {
                    EmailAddress = user.Email,
                    Subject = $"Opnieuw opgeven beschrikbaarheid datumprikker {dbDatepicker.Title}",
                    Message = $"Beste {user.Username},{Environment.NewLine}" +
                $"Een aantal datums van de datumprikker {dbDatepicker.Title} zijn aangepast. Daarom moet je opnieuw je beschikbaarheid opgeven."
                })
                .ToList();

            _publisher.Publish(emails, RabbitMqRouting.SendMail, RabbitMqExchange.MailExchange);
        }

        /// <summary>
        /// Deletes the datepicker in the database by uuid
        /// </summary>
        /// <param name="uuid">The uuid to delete</param>
        public async Task Delete(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                throw new UnprocessableException();
            }

            DatepickerDto dbDatepicker = await _datepickerDal.Find(uuid);
            if (dbDatepicker == null)
            {
                throw new KeyNotFoundException();
            }

            await _datepickerDal.Delete(uuid);

            // Inform users about the update
            List<Guid> userUuidCollection = dbDatepicker.Dates.SelectMany(d => d.UserAvailabilities.Select(ua => ua.UserUuid))
                .ToList();

            if (!userUuidCollection.Any())
            {
                return;
            }

            var rpcClient = new RpcClient(_model);
            var users = rpcClient.Call<List<UserRabbitMq>>(userUuidCollection, RabbitMqRouting.FindUser);

            var emails = users
                .Select(user => new EmailRabbitMq
                {
                    EmailAddress = user.Email,
                    Subject = $"datumprikker { dbDatepicker.Title } verwijderd",
                    Message = $"Beste {user.Username},{Environment.NewLine}" +
                              $"De datumprikker {dbDatepicker.Title} is verwijderd."
                })
                .ToList();

            _publisher.Publish(emails, RabbitMqRouting.SendMail, RabbitMqExchange.MailExchange);
        }
    }
}