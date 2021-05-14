using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using User_Service.CustomExceptions;
using User_Service.Dal.Interfaces;
using User_Service.Models;
using User_Service.Models.RabbitMq;

namespace User_Service.Logic
{
    public class ActivationLogic
    {
        private readonly IActivationDal _activationDal;
        private readonly IDisabledUserDal _disabledUserDal;
        private readonly IMapper _mapper;

        public ActivationLogic(IActivationDal activationDal, IDisabledUserDal disabledUserDal, IMapper mapper)
        {
            _activationDal = activationDal;
            _disabledUserDal = disabledUserDal;
            _mapper = mapper;
        }

        private static bool ActivationModelValid(ActivationDto activation)
        {
            return !string.IsNullOrEmpty(activation.Code) &&
                   activation.UserUuid != Guid.Empty &&
                   activation.Uuid != Guid.Empty;
        }

        public async Task Add(UserActivationRabbitMq activation)
        {
            var activationDto = _mapper.Map<ActivationDto>(activation);
            if (!ActivationModelValid(activationDto))
            {
                throw new UnprocessableException();
            }

            await _activationDal.Add(activationDto);
        }

        public async Task ActivateUser(string code)
        {
            ActivationDto activationDto = await _activationDal.Find(code);
            if (activationDto == null)
            {
                throw new KeyNotFoundException();
            }

            await _disabledUserDal.Delete(activationDto.UserUuid);
            await _activationDal.Delete(activationDto.Uuid);
        }
    }
}