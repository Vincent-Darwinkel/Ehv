using AutoMapper;
using Event_Service.Logic;
using Event_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Mvc;

namespace Event_Service.Controllers
{
    [Route("event/step")]
    [ApiController]
    public class EventStepController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly LogLogic _logLogic;
        private readonly ControllerHelper _controllerHelper;
        private readonly EventStepLogic _eventStepLogic;

        public EventStepController(IMapper mapper, LogLogic logLogic,
            ControllerHelper controllerHelper, EventStepLogic eventStepLogic)
        {
            _mapper = mapper;
            _logLogic = logLogic;
            _controllerHelper = controllerHelper;
            _eventStepLogic = eventStepLogic;
        }
    }
}