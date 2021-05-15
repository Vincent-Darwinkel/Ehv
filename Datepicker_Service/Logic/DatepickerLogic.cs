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
using AutoMapper;

namespace Datepicker_Service.Logic
{
    public class DatepickerLogic
    {
        private readonly IDatepickerDal _datepickerDal;
        private readonly IModel _channel;
        private readonly IPublisher _publisher;
        private readonly RpcClient _rpcClient;
        private readonly IMapper _mapper;
        private readonly IDatepickerDateDal _datepickerDateDal;

        public DatepickerLogic(IDatepickerDal datepickerDal, IModel channel,
            IPublisher publisher, RpcClient rpcClient, IMapper mapper, IDatepickerDateDal datepickerDateDal)
        {
            _datepickerDal = datepickerDal;
            _channel = channel;
            _publisher = publisher;
            _rpcClient = rpcClient;
            _mapper = mapper;
            _datepickerDateDal = datepickerDateDal;
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
        public async Task Add(DatepickerDto datepicker, UserHelper requestingUser)
        {
            datepicker.Uuid = Guid.NewGuid();
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

        private static bool DatepickerConversionModelValid(DatePickerConversion datePickerConversion)
        {
            return datePickerConversion.SelectedDates.Count > 0 &&
                   datePickerConversion.DatepickerUuid != Guid.Empty;
        }

        public async Task ConvertDatepicker(DatePickerConversion datePickerConversion, UserHelper requestingUser)
        {
            if (!DatepickerConversionModelValid(datePickerConversion))
            {
                throw new UnprocessableException();
            }

            DatepickerDto dbDatepicker = await _datepickerDal.Find(datePickerConversion.DatepickerUuid);
            if (dbDatepicker == null)
            {
                throw new NoNullAllowedException();
            }

            if (dbDatepicker.AuthorUuid != requestingUser.Uuid)
            {
                throw new UnauthorizedAccessException();
            }

            bool datesAreInDatePicker = datePickerConversion.SelectedDates
                .TrueForAll(dateUuid => dbDatepicker.Dates
                    .Exists(dbDate => dbDate.Uuid == dateUuid));

            if (!datesAreInDatePicker)
            {
                throw new UnprocessableException();
            }

            var datepickerRabbitMq = _mapper.Map<DatepickerRabbitMq>(dbDatepicker);
            datepickerRabbitMq.EventSteps = _mapper.Map<List<EventStepRabbitMq>>(datePickerConversion.EventSteps);
            datepickerRabbitMq.SelectedDates = datePickerConversion.SelectedDates;
            datepickerRabbitMq.Dates
                .RemoveAll(d => !datePickerConversion.SelectedDates
                    .Contains(d.Uuid));

            _publisher.Publish(datepickerRabbitMq, RabbitMqRouting.ConvertDatepicker, RabbitMqExchange.ConvertDatepicker);
            await _datepickerDal.Delete(datePickerConversion.DatepickerUuid);
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

            DatepickerDto datepicker = await _datepickerDal.Find(uuid);
            if (datepicker == null)
            {
                throw new KeyNotFoundException();
            }

            return datepicker;
        }

        public async Task<List<DatepickerDto>> All()
        {
            return await _datepickerDal.All();
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
                bool datepickerWithNameExists = await _datepickerDal.Exists(datepicker.Title);
                if (datepickerWithNameExists)
                {
                    throw new DuplicateNameException();
                }

                var rpcClient = new RpcClient(_channel);
                bool eventNameExists = rpcClient.Call<bool>(datepicker.Title, RabbitMqQueues.ExistsEventQueue);
                if (eventNameExists)
                {
                    throw new DuplicateNameException();
                }
            }

            await UpdateDatabaseDatepicker(datepicker, dbDatepicker);

            if (datepicker.Dates.Count != dbDatepicker.Dates.Count || datepicker.Dates // update datepicker dates if supplied dates date time is different from database values
                .TrueForAll(dpd => dbDatepicker.Dates
                    .All(dbDpd => dbDpd.DateTime != dpd.DateTime)))
            {
                await _datepickerDateDal.Delete(dbDatepicker.Dates);
                await _datepickerDateDal.Add(datepicker.Dates);
                if (userUuidCollection.Any())
                {
                    var rpcClient = new RpcClient(_channel);
                    InformUsersAboutDatepickerDatesUpdate(rpcClient, userUuidCollection, dbDatepicker);
                }
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
