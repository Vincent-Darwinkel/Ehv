using AutoMapper;
using Logging_Service.Models.FromFrontend;
using Logging_Service.Models.RabbitMq;

namespace Logging_Service.Models.Helpers
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LogRabbitMq, LogDto>();
            cfg.CreateMap<LogDto, Log>();
        });
    }
}