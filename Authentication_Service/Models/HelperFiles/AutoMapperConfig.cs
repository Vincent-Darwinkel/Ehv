using Authentication_Service.Models.Dto;
using Authentication_Service.Models.RabbitMq;
using AutoMapper;

namespace Authentication_Service.Models.HelperFiles
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserRabbitMqSensitiveInformation, UserDto>();
        });
    }
}