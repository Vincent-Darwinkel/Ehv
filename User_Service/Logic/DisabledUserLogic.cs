using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using User_Service.CustomExceptions;
using User_Service.Dal;
using User_Service.Dal.Interfaces;
using User_Service.Enums;
using User_Service.Models;
using User_Service.Models.FromFrontend;
using User_Service.Models.RabbitMq;

namespace User_Service.Logic
{
    public class DisabledUserLogic
    {
        private readonly IDisabledUserDal _disabledUserDal;
        private readonly IMapper _mapper;
        private readonly IUserDal _userDal;

        public DisabledUserLogic(IDisabledUserDal disabledUserDal, IMapper mapper, IUserDal userDal)
        {
            _disabledUserDal = disabledUserDal;
            _mapper = mapper;
            _userDal = userDal;
        }

        private static bool DisabledUserModelValid(DisabledUserDto disabledUser)
        {
            return disabledUser.Reason != DisableReason.Undefined &&
                   disabledUser.UserUuid != Guid.Empty &&
                   disabledUser.Uuid != Guid.Empty;
        }

        public async Task Add(DisabledUser disabledUser)
        {
            var disabledUserDto = _mapper.Map<DisabledUserDto>(disabledUser);
            if (!DisabledUserModelValid(disabledUserDto))
            {
                throw new UnprocessableException();
            }

            disabledUserDto.Uuid = Guid.NewGuid();
            await _disabledUserDal.Add(disabledUserDto);
        }

        public async Task Add(DisabledUserRabbitMq disabledUser)
        {
            var disabledUserDto = _mapper.Map<DisabledUserDto>(disabledUser);
            if (!DisabledUserModelValid(disabledUserDto))
            {
                throw new UnprocessableException();
            }

            await _disabledUserDal.Add(disabledUserDto);
        }

        public async Task<List<UserDto>> All()
        {
            List<Guid> disabledUserUuidCollection = await _disabledUserDal.All();
            return await _userDal.Find(disabledUserUuidCollection);
        }

        public async Task<string> Exists(string json)
        {
            Guid userUuid = Newtonsoft.Json.JsonConvert.DeserializeObject<Guid>(json);
            if (userUuid == Guid.Empty)
            {
                throw new UnprocessableException();
            }

            bool exists = await _disabledUserDal.Exists(userUuid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(exists);
        }

        public async Task Delete(Guid userUuid)
        {
            if (userUuid == Guid.Empty)
            {
                throw new UnprocessableException();
            }

            await _disabledUserDal.Delete(userUuid);
        }
    }
}