using Authentication_Service.CustomExceptions;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Enums;
using Authentication_Service.Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Authentication_Service.Models.HelperFiles;
using Authentication_Service.Models.RabbitMq;
using Authentication_Service.RabbitMq.Publishers;
using Authentication_Service.RabbitMq.Rpc;

namespace Authentication_Service.Logic
{
    public class UserLogic
    {
        private readonly IUserDal _userDal;
        private readonly IDisabledUserDal _disabledUserDal;
        private readonly IActivationDal _activationDal;
        private readonly SecurityLogic _securityLogic;
        private readonly IPublisher _publisher;
        private readonly RpcClient _rpcClient;

        public UserLogic(IUserDal userDal, IDisabledUserDal disabledUserDal, IActivationDal activationDal,
            SecurityLogic securityLogic, IPublisher publisher, RpcClient rpcClient)
        {
            _userDal = userDal;
            _disabledUserDal = disabledUserDal;
            _activationDal = activationDal;
            _securityLogic = securityLogic;
            _publisher = publisher;
            _rpcClient = rpcClient;
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
            await _userDal.Add(user);
            await _disabledUserDal.Add(new DisabledUserDto
            {
                Reason = DisableReason.EmailVerificationRequired,
                UserUuid = user.Uuid,
                Uuid = Guid.NewGuid()
            });

            var activationDto = new ActivationDto
            {
                Code = Guid.NewGuid().ToString(),
                UserUuid = user.Uuid,
                Uuid = Guid.NewGuid()
            };

            await _activationDal.Add(activationDto);

            var userFromUserService = _rpcClient.Call<List<UserRabbitMq>>(new List<Guid>
            {
                user.Uuid
            }, RabbitMqQueues.FindUserQueue).FirstOrDefault();

            if (userFromUserService == null)
            {
                throw new NoNullAllowedException();
            }

            var email = new EmailRabbitMq
            {
                EmailAddress = userFromUserService.Email,
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
