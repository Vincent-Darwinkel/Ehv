using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Event_Service.Models.HelperFiles;
using Event_Service.Models.RabbitMq;
using Event_Service.Models.ToFrontend;
using Event_Service.RabbitMq.Publishers;
using Event_Service.RabbitMq.Rpc;

namespace Event_Service.Logic
{
    public class EventLogic
    {
        private readonly IEventDal _eventDal;
        private readonly IMapper _mapper;
        private readonly RpcClient _rpcClient;
        private readonly IPublisher _publisher;

        public EventLogic(IEventDal eventDal, IMapper mapper, RpcClient rpcClient, IPublisher publisher)
        {
            _eventDal = eventDal;
            _mapper = mapper;
            _rpcClient = rpcClient;
            _publisher = publisher;
        }

        public async Task<List<EventDto>> All(UserHelper requestingUser)
        {
            List<EventDto> events = await _eventDal.All();
            if (events == null)
            {
                throw new NoNullAllowedException(nameof(events));
            }

            // remove the uuid of the users
            foreach (var dbEvent in events.Where(dbEvent => dbEvent.AuthorUuid != requestingUser.Uuid))
            {
                dbEvent.AuthorUuid = Guid.Empty;
            }

            return events;
        }

        private static List<Guid> GetUserUuidsFromEvent(EventViewmodel eventViewModel)
        {
            var userUuidCollection = new List<Guid>();

            userUuidCollection.AddRange(eventViewModel.EventDates
                .SelectMany(ed => ed.EventDateUsers)
                .Select(edu => edu.UserUuid));
            userUuidCollection.AddRange(eventViewModel.EventSteps
                .SelectMany(es => es.EventStepUsers
                    .Select(esu => esu.UserUuid)));

            return userUuidCollection
                .Distinct()
                .ToList();
        }

        public async Task<EventViewmodel> FindAsync(Guid eventUuid, UserHelper requestingUser)
        {
            EventDto dbEvent = await _eventDal.Find(eventUuid);
            if (dbEvent == null)
            {
                throw new NoNullAllowedException(nameof(dbEvent));
            }

            var mappedEvent = _mapper.Map<EventViewmodel>(dbEvent);
            List<Guid> userUuidCollection = GetUserUuidsFromEvent(mappedEvent);

            var usersFromUserService = _rpcClient.Call<List<UserRabbitMq>>(userUuidCollection, RabbitMqQueues.FindUserQueue);
            AddUsernamesToEventDate(requestingUser, mappedEvent, usersFromUserService);
            AddUsernamesToEventSteps(requestingUser, mappedEvent, usersFromUserService);

            mappedEvent.CanBeRemoved = dbEvent.AuthorUuid != Guid.Empty &&
                                       dbEvent.AuthorUuid == requestingUser.Uuid;
            return mappedEvent;
        }

        private static void AddUsernamesToEventSteps(UserHelper requestingUser, EventViewmodel mappedEvent,
            List<UserRabbitMq> usersFromUserService)
        {
            foreach (var eventStep in mappedEvent.EventSteps)
            {
                eventStep.Completed = eventStep.EventStepUsers.Any(esu => esu.UserUuid == requestingUser.Uuid);
                foreach (var eventStepUser in eventStep.EventStepUsers)
                {
                    eventStepUser.Username = usersFromUserService
                        .Find(u => u.Uuid == eventStepUser.UserUuid)
                        .Username;
                }
            }
        }

        private static void AddUsernamesToEventDate(UserHelper requestingUser, EventViewmodel mappedEvent,
            List<UserRabbitMq> usersFromUserService)
        {
            foreach (var eventDate in mappedEvent.EventDates)
            {
                eventDate.Subscribed = eventDate.EventDateUsers.Any(edu => edu.UserUuid == requestingUser.Uuid);
                foreach (var eventDateUser in eventDate.EventDateUsers)
                {
                    eventDateUser.Username = usersFromUserService
                        .Find(u => u.Uuid == eventDateUser.UserUuid)
                        .Username;
                }
            }
        }

        public async Task ConvertToEventAsync(DatepickerRabbitMq datepickerRabbitMq, UserHelper requestingUser)
        {
            var selectedDates = datepickerRabbitMq.Dates
                .FindAll(date => datepickerRabbitMq.SelectedDates
                    .Exists(dateUuid => dateUuid == date.Uuid));

            var eventDates = _mapper.Map<List<EventDateDto>>(selectedDates);
            var eventDto = new EventDto
            {
                AuthorUuid = datepickerRabbitMq.AuthorUuid,
                Description = datepickerRabbitMq.Description,
                EventDates = eventDates,
            };

            datepickerRabbitMq.EventSteps
                .ForEach(es => eventDto.EventSteps.Add(new EventStepDto
                {
                    Uuid = Guid.NewGuid(),
                    Description = es.Text,
                    EventUuid = eventDto.Uuid,
                    StepNr = es.StepNr
                }));

            AddDatesAndAvailableUsersToEvent(selectedDates, datepickerRabbitMq, eventDto);
            await _eventDal.Add(eventDto);

            List<Guid> usersToNotifyUuidCollection = eventDto.EventDates
                .SelectMany(e => e.EventDateUsers
                    .Select(u => u.UserUuid))
                .Distinct()
                .ToList();

            if (usersToNotifyUuidCollection.Count > 0)
            {
                NotifyUsersAboutConversion(usersToNotifyUuidCollection, eventDto);
            }
        }

        public async Task<string> Exists(string title)
        {
            bool exists = await _eventDal.Exists(title);
            return Newtonsoft.Json.JsonConvert.SerializeObject(exists);
        }

        private static void AddDatesAndAvailableUsersToEvent(List<DatepickerDateRabbitMq> selectedDates, DatepickerRabbitMq datepicker,
            EventDto eventToInsert)
        {
            selectedDates.ForEach(sd =>
            {
                var eventDate = new EventDateDto();
                var eventDateUsers = new List<EventDateUserDto>();

                datepicker.Dates
                    .Find(d => d.Uuid == sd.Uuid)
                    .UserAvailabilities.ForEach(user =>
                    {
                        eventDateUsers.Add(new EventDateUserDto
                        {
                            EventDateUuid = eventDate.Uuid,
                            UserUuid = user.UserUuid
                        });
                    });

                eventToInsert.EventDates.Add(new EventDateDto
                {
                    DateTime = sd.DateTime,
                    EventDateUsers = eventDateUsers
                });
            });
        }

        private void NotifyUsersAboutConversion(List<Guid> usersToNotifyUuidCollection, EventDto eventToInsert)
        {
            var usersRabbitMq = _rpcClient.Call<List<UserRabbitMq>>(usersToNotifyUuidCollection, RabbitMqQueues.FindUserQueue);
            var emails = usersRabbitMq.Select(user => new EmailRabbitMq
            {
                TemplateName = "DatepickerConversion",
                EmailAddress = user.Email,
                KeyWordValues = new List<EmailKeyWordValue>
                    {
                        new EmailKeyWordValue {Key = "Username", Value = user.Username},
                        new EmailKeyWordValue {Key = "DatepickerTitle", Value = eventToInsert.Title},
                        new EmailKeyWordValue
                        {
                            Key = "DatepickerDates",
                            Value = string.Join(",", eventToInsert.EventDates.Select(e => e.DateTime)
                                .ToArray())
                        }
                    }
            })
                .ToList();

            _publisher.Publish(emails, RabbitMqRouting.SendMail, RabbitMqExchange.MailExchange);
        }

        public async Task RemoveAsync(Guid eventToCancelUuid, UserHelper requestingUser)
        {
            EventDto dbEvent = await _eventDal.Find(eventToCancelUuid);
            if (dbEvent == null)
            {
                throw new NoNullAllowedException(nameof(dbEvent));
            }
            if (dbEvent.AuthorUuid != requestingUser.Uuid)
            {
                throw new UnauthorizedAccessException();
            }

            List<Guid> usersToNotify = dbEvent.EventDates
                .SelectMany(e => e.EventDateUsers
                    .Select(u => u.UserUuid))
                    .ToList();

            NotifyUsersAboutDeletedEvent(usersToNotify, dbEvent.Title);
            await _eventDal.Delete(dbEvent);
        }

        private void NotifyUsersAboutDeletedEvent(List<Guid> userUuidCollection, string eventName)
        {
            var usersRabbitMq = _rpcClient.Call<List<UserRabbitMq>>(userUuidCollection, RabbitMqQueues.FindUserQueue);
            var emails = usersRabbitMq.Select(user => new EmailRabbitMq
            {
                TemplateName = "DeleteEvent",
                EmailAddress = user.Email,
                KeyWordValues = new List<EmailKeyWordValue>
                    {
                        new EmailKeyWordValue {Key = "Username", Value = user.Username},
                        new EmailKeyWordValue {Key = "EventName", Value = eventName},
                    }
            })
                .ToList();

            _publisher.Publish(emails, RabbitMqRouting.SendMail, RabbitMqExchange.MailExchange);
        }
    }
}