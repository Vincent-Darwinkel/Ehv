using Authentication_Service.CustomExceptions;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Enums;
using Authentication_Service.Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authentication_Service.Models.HelperFiles;
using Authentication_Service.Models.RabbitMq;
using Authentication_Service.RabbitMq.Publishers;
using DisableReason = User_Service.Enums.DisableReason;

namespace Authentication_Service.Logic
{
    public class UserLogic
    {
        private readonly IUserDal _userDal;
        private readonly IDisabledUserDal _disabledUserDal;
        private readonly IActivationDal _activationDal;
        private readonly SecurityLogic _securityLogic;
        private readonly IPublisher _publisher;

        public UserLogic(IUserDal userDal, IDisabledUserDal disabledUserDal, IActivationDal activationDal,
            SecurityLogic securityLogic, IPublisher publisher)
        {
            _userDal = userDal;
            _disabledUserDal = disabledUserDal;
            _activationDal = activationDal;
            _securityLogic = securityLogic;
            _publisher = publisher;
        }

        private bool UserModelValid(UserDto user)
        {
            return user.Uuid != Guid.Empty &&
                   !string.IsNullOrEmpty(user.Username) &&
                   !string.IsNullOrEmpty(user.Password) &&
                   user.AccountRole != AccountRole.Undefined;
        }

        public async Task Add(UserDto user)
        {
            if (!UserModelValid(user))
            {
                throw new UnprocessableException();
            }

            user.Password = _securityLogic.HashPassword(user.Password);
            /* await _userDal.Add(user);
             await _disabledUserDal.Add(new DisabledUserDto
             {
                 Reason = DisableReason.EmailVerificationRequired,
                 UserUuid = user.Uuid,
                 Uuid = Guid.NewGuid()
             });*/

            var activationDto = new ActivationDto
            {
                Code = Guid.NewGuid().ToString(),
                UserUuid = user.Uuid,
                Uuid = Guid.NewGuid()
            };

            //await _activationDal.Add(activationDto);

            var email = new EmailRabbitMq
            {
                UserUuid = user.Uuid,
                TemplateName = "ActivateAccount",
                Subject = "Activatie Eindhovense vriendjes",
                KeyWordValues = new List<EmailKeyWordValue>
                {
                    new EmailKeyWordValue
                    {
                        Key = "Username",
                        Value = user.Username
                    },
                    new EmailKeyWordValue
                    {
                        Key = "ActivationCode",
                        Value = activationDto.Code
                    }
                }
            };

            _publisher.Publish(new List<EmailRabbitMq> { email }, RabbitMqRouting.SendMail, RabbitMqExchange.MailExchange);
        }

        public async Task Update(UserDto user)
        {
            if (!UserModelValid(user))
            {
                throw new UnprocessableException();
            }

            UserDto dbUser = await _userDal.Find(user.Uuid);
            dbUser.Username = user.Username;
            dbUser.AccountRole = user.AccountRole;
            if (user.Password != dbUser.Password)
            {
                dbUser.Password = _securityLogic.HashPassword(user.Password);
            }

            await _userDal.Update(dbUser);
        }

        public async Task Delete(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                throw new UnprocessableException();
            }

            await _userDal.Delete(uuid);
        }
    }
}
