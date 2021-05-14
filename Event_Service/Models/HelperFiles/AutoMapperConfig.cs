using AutoMapper;
using Event_Service.Models.RabbitMq;

namespace Event_Service.Models.HelperFiles
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DatepickerDateRabbitMq, EventDateDto>();
        });
    }
}
