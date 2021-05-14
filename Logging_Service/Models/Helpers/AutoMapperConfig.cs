using AutoMapper;
using Logging_Service.Models.RabbitMq;
using Logging_Service.Models.ToFrontend;

namespace Logging_Service.Models.Helpers
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LogRabbitMq, LogDto>();
            cfg.CreateMap<LogDto, LogViewmodel>();
        });
    }
}