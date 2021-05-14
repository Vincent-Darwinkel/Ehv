using AutoMapper;
using Event_Service.Logic;
using Event_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Mvc;

namespace Event_Service.Controllers
{
    [Route("event/date/")]
    [ApiController]
    public class EventDateController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly LogLogic _logLogic;
        private readonly ControllerHelper _controllerHelper;
        private readonly EventDateLogic _eventDateLogic;

        public EventDateController(IMapper mapper, LogLogic logLogic,
            ControllerHelper controllerHelper, EventDateLogic eventDateLogic)
        {
            _mapper = mapper;
            _logLogic = logLogic;
            _controllerHelper = controllerHelper;
            _eventDateLogic = eventDateLogic;
        }
    }
}
