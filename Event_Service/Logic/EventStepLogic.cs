using Event_Service.Dal.Interfaces;

namespace Event_Service.Logic
{
    public class EventStepLogic
    {
        private readonly IEventStepDal _eventStepDal;

        public EventStepLogic(IEventStepDal eventStepDal)
        {
            _eventStepDal = eventStepDal;
        }
    }
}
