using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication_Service.CustomExceptions;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Enums;
using Authentication_Service.Models.Dto;
using Authentication_Service.Models.FromFrontend;
using Authentication_Service.Models.HelperFiles;
using Authentication_Service.Models.RabbitMq;
using Authentication_Service.Models.ToFrontend;
using Authentication_Service.RabbitMq.Publishers;
using Authentication_Service.RabbitMq.Rpc;
using UserDto = Authentication_Service.Models.Dto.UserDto;

namespace Authentication_Service.Logic
{
    public class AuthenticationLogic
    {
        private readonly IUserDal _userDal;
        private readonly IPublisher _publisher;
        private readonly IPendingLoginDal _pendingLoginDal;
        private readonly SecurityLogic _securityLogic;
        private readonly JwtLogic _jwtLogic;
        private readonly RpcClient _rpcClient;

        public AuthenticationLogic(IUserDal userDal, IPublisher publisher, IPendingLoginDal pendingLoginDal,
            SecurityLogic securityLogic, JwtLogic jwtLogic, RpcClient rpcClient)
        {
            _userDal = userDal;
            _publisher = publisher;
            _pendingLoginDal = pendingLoginDal;
            _securityLogic = securityLogic;
            _jwtLogic = jwtLogic;
            _rpcClient = rpcClient;
        }

        /// <summary>
        /// Checks if the credentials are correct and returns an jwt and refresh token if password is correct
        /// </summary>
        /// <param name="login">The username and password</param>
        /// <returns>An jwt and refresh token if password is correct, if not correct null is returned</returns>
        public async Task<LoginResultViewmodel> Login(Login login)
        {
            UserDto dbUser = await _userDal.Find(login.Username);
            if (dbUser == null)
            {
                throw new UnauthorizedAccessException();
            }

            bool userIsDisabled = _rpcClient.Call<bool>(dbUser.Uuid, RabbitMqQueues.DisabledExistsUserQueue);
            if (userIsDisabled)
            {
                throw new DisabledUserException();
            }

            bool passwordCorrect = _securityLogic.VerifyPassword(login.Password, dbUser.Password);
            if (!passwordCorrect)
            {
                throw new UnauthorizedAccessException();
            }

            if (login.LoginCode > 99999 && login.LoginCode < 1000000 && login.SelectedAccountRole != AccountRole.Undefined)
            {
                return await LoginWithSelectedAccount(login, dbUser);
            }

            if (dbUser.AccountRole > AccountRole.User)
            {
                return await HandleMultipleAccountRolesLogin(dbUser);
            }

            AuthorizationTokensViewmodel tokens = await _jwtLogic.CreateJwt(dbUser);
            return new LoginResultViewmodel
            {
                Jwt = tokens.Jwt,
                RefreshToken = tokens.RefreshToken
            };
        }

        private async Task<LoginResultViewmodel> LoginWithSelectedAccount(Login login, UserDto user)
        {
            PendingLoginDto dbPendingLogin = await _pendingLoginDal.Find(new PendingLoginDto
            {
                UserUuid = user.Uuid,
                AccessCode = login.LoginCode
            });

            if (dbPendingLogin == null || dbPendingLogin.ExpirationDate < DateTime.Now)
            {
                throw new UnauthorizedAccessException(nameof(login));
            }

            user.AccountRole = login.SelectedAccountRole;
            await _pendingLoginDal.Remove(dbPendingLogin);
            await _pendingLoginDal.RemoveOutdated();

            AuthorizationTokensViewmodel tokens = await _jwtLogic.CreateJwt(user);
            return new LoginResultViewmodel
            {
                Jwt = tokens.Jwt,
                RefreshToken = tokens.RefreshToken,
            };
        }

        /// <summary>
        /// The user has an possibility to login with multiple account roles if the account is admin or site admin
        /// This is implemented so that the user does not need multiple accounts for every functionality
        /// </summary>
        /// <param name="user">The user from the database</param>
        /// <returns>The login result</returns>
        private async Task<LoginResultViewmodel> HandleMultipleAccountRolesLogin(UserDto user)
        {
            var pendingLogin = new PendingLoginDto
            {
                UserUuid = user.Uuid
            };

            var userFromUserService = _rpcClient.Call<List<UserRabbitMqSensitiveInformation>>(new List<Guid>
            {
                user.Uuid
            }, RabbitMqQueues.FindUserQueue).FirstOrDefault();

            var email = new EmailRabbitMq
            {
                EmailAddress = userFromUserService.Email,
                Subject = "Login code",
                TemplateName = "LoginMultiRole",
                KeyWordValues = new List<EmailKeyWordValue>
                {
                    new EmailKeyWordValue
                    {
                        Key = "Username",
                        Value = user.Username
                    },
                    new EmailKeyWordValue
                    {
                        Key = "LoginCode",
                        Value = pendingLogin.AccessCode.ToString()
                    }
                }
            };

            _publisher.Publish(new List<EmailRabbitMq> { email }, RabbitMqRouting.SendMail, RabbitMqExchange.MailExchange);

            await _pendingLoginDal.Remove(pendingLogin.UserUuid);
            await _pendingLoginDal.RemoveOutdated();
            await _pendingLoginDal.Add(pendingLogin);

            List<AccountRole> allAccountRoles = Enum.GetValues(typeof(AccountRole))
                .Cast<AccountRole>()
                .ToList();

            return new LoginResultViewmodel
            {
                UserHasMultipleAccountRoles = true,
                SelectableAccountRoles = allAccountRoles
                    .FindAll(aa => aa != AccountRole.Undefined && aa <= user.AccountRole)
            };
        }
    }
}