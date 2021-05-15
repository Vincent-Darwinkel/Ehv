using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Event_Service.Models.HelperFiles;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Event_Service.Logic
{
    public class EventStepUserLogic
    {
        private readonly IEventStepUserDal _eventStepUserDal;
        private readonly IEventStepDal _eventStepDal;

        public EventStepUserLogic(IEventStepUserDal eventStepUserDal, IEventStepDal eventStepDal)
        {
            _eventStepUserDal = eventStepUserDal;
            _eventStepDal = eventStepDal;
        }

        /// <summary>
        /// Saves the specified progress of a step from the specified requestingUser
        /// </summary>
        /// <param name="stepUuid">The step uuid</param>
        /// <param name="requestingUser">The user that made the request</param>
        /// <returns></returns>
        public async Task Add(Guid stepUuid, UserHelper requestingUser)
        {
            if (await _eventStepUserDal.Find(stepUuid, requestingUser.Uuid) != null)
            {
                throw new DuplicateNameException(nameof(stepUuid.ToString));
            }

            await _eventStepUserDal.Add(new EventStepUserDto
            {
                EventStepUuid = stepUuid,
                UserUuid = requestingUser.Uuid,
                Uuid = Guid.NewGuid()
            });
        }

        public async Task Remove(Guid stepUuid, UserHelper requestingUser)
        {
            EventStepDto dbEventStep = await _eventStepDal.Find(stepUuid);
            EventStepUserDto dbEventStepUser = dbEventStep.EventStepUsers
                .Find(esu => esu.UserUuid == requestingUser.Uuid);

            if (dbEventStepUser == null)
            {
                throw new NoNullAllowedException();
            }

            await _eventStepUserDal.Remove(dbEventStepUser);
        }
    }
}