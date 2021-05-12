using Datepicker_Service.CustomExceptions;
using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.Models;
using Datepicker_Service.Models.FromFrontend;
using Datepicker_Service.Models.HelperFiles;
using Datepicker_Service.Models.RabbitMq;
using Datepicker_Service.RabbitMq.Publishers;
using Datepicker_Service.RabbitMq.Rpc;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Datepicker_Service.Logic
{
    public class DatepickerLogic
    {
        private readonly IDatepickerDal _datepickerDal;
        private readonly IModel _channel;
        private readonly IPublisher _publisher;
        private readonly RpcClient _rpcClient;

        public DatepickerLogic(IDatepickerDal datepickerDal, IModel channel, IPublisher publisher, RpcClient rpcClient)
        {
            _datepickerDal = datepickerDal;
            _channel = channel;
            _publisher = publisher;
            _rpcClient = rpcClient;
        }

        private bool DatepickerValid(DatepickerDto datepicker)
        {
            return !string.IsNullOrEmpty(datepicker.Title) &&
                   !string.IsNullOrEmpty(datepicker.Description) &&
                   !string.IsNullOrEmpty(datepicker.Location) &&
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

            bool datepickerExists = await _datepickerDal.Exists(datepicker.Title);
            bool eventExists = _rpcClient.Call<bool>(datepicker.Title, RabbitMqQueues.ExistsEventQueue);
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
            if (dbDatepicker == null)
            {
                throw new KeyNotFoundException();
            }

            List<Guid> userUuidCollection = dbDatepicker.Dates.SelectMany(d => d.UserAvailabilities.Select(ua => ua.UserUuid))
                .ToList();

            if (datepicker.Title != dbDatepicker.Title)
            {
                var rpcClient = new RpcClient(_channel);
                bool eventNameExists = rpcClient.Call<bool>(datepicker.Title, RabbitMqQueues.ExistsEventQueue);
                if (eventNameExists)
                {
                    throw new DuplicateNameException();
                }
            }

            await UpdateDatabaseDatepicker(datepicker, dbDatepicker);
            if (userUuidCollection.Count <= 0)
            {
                return;
            }
            if (datepicker.Dates.Count != dbDatepicker.Dates.Count || datepicker.Dates // update datepicker dates if supplied dates date time is different from database values
                .TrueForAll(dpd => dbDatepicker.Dates
                    .All(dbDpd => dbDpd.DateTime != dpd.DateTime)))
            {
                dbDatepicker.Dates = datepicker.Dates;

                var rpcClient = new RpcClient(_channel);
                InformUsersAboutDatepickerDatesUpdate(rpcClient, userUuidCollection, dbDatepicker);
            }
        }

        private void InformUsersAboutDatepickerDatesUpdate(RpcClient rpcClient, List<Guid> userUuidCollection,
            DatepickerDto dbDatepicker)
        {
            var users = rpcClient.Call<List<UserRabbitMq>>(userUuidCollection, RabbitMqQueues.FindUserQueue);

            var emails = users
                .Select(user => new EmailRabbitMq
                {
                    EmailAddress = user.Email,
                    Subject = $"Opnieuw opgeven beschikbaarheid datumprikker {dbDatepicker.Title}",
                    Message = $"Beste {user.Username},{Environment.NewLine}" +
                              $"Een aantal datums van de datumprikker {dbDatepicker.Title} zijn aangepast. Daarom moet je opnieuw je beschikbaarheid opgeven."
                })
                .ToList();

            _publisher.Publish(emails, RabbitMqRouting.SendMail, RabbitMqExchange.MailExchange);
        }

        private async Task UpdateDatabaseDatepicker(DatepickerDto datepicker, DatepickerDto dbDatepicker)
        {
            dbDatepicker.Title = datepicker.Title;
            dbDatepicker.Description = datepicker.Description;
            dbDatepicker.Expires = datepicker.Expires;
            dbDatepicker.Location = datepicker.Location;
            await _datepickerDal.Update(dbDatepicker);
        }

        /// <summary>
        /// Deletes the datepicker in the database by uuid
        /// </summary>
        /// <param name="uuid">The uuid to delete</param>
        public async Task Delete(Guid uuid, Guid requestingUserUuid)
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

            if (dbDatepicker.AuthorUuid != requestingUserUuid)
            {
                throw new UnauthorizedAccessException();
            }

            await _datepickerDal.Delete(uuid);

            // Inform users about the update
            List<Guid> userUuidCollection = dbDatepicker.Dates.SelectMany(d => d.UserAvailabilities.Select(ua => ua.UserUuid))
                .ToList();

            if (!userUuidCollection.Any())
            {
                return;
            }

            var rpcClient = new RpcClient(_channel);
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
