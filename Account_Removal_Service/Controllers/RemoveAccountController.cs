using Account_Removal_Service.Enums;
using Account_Removal_Service.Logic;
using Account_Removal_Service.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Account_Removal_Service.Controllers
{
    [AuthorizedAction(new[] { AccountRole.User, AccountRole.Admin })]
    [Route("account-removal")]
    [ApiController]
    public class RemoveAccountController : ControllerBase
    {
        private readonly AccountRemovalLogic _accountRemovalLogic;
        private readonly LogLogic _logLogic;
        private readonly ControllerHelper _controllerHelper;

        public RemoveAccountController(AccountRemovalLogic accountRemovalLogic, LogLogic logLogic, ControllerHelper controllerHelper)
        {
            _accountRemovalLogic = accountRemovalLogic;
            _logLogic = logLogic;
            _controllerHelper = controllerHelper;
        }

        [HttpPost]
        public ActionResult RemoveUserData([FromQuery(Name = "options")] List<RemovableOptions> options)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                _accountRemovalLogic.RemoveAccount(new AccountRemoval
                {
                    DataToRemove = options,
                    UserUuid = requestingUser.Uuid
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}