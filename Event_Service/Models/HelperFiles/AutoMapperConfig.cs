using AutoMapper;
using Event_Service.Models.RabbitMq;
using Event_Service.Models.ToFrontend;

namespace Event_Service.Models.HelperFiles
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DatepickerRabbitMq, EventDto>();
            cfg.CreateMap<DatepickerDateRabbitMq, EventDateDto>()
                .ForMember(da => da.EventDateUsers, opt =>
                    opt.MapFrom(src => src.UserAvailabilities));
            cfg.CreateMap<DatepickerAvailabilityRabbitMq, EventDateUserDto>();
            cfg.CreateMap<EventStepRabbitMq, EventStepDto>();
            cfg.CreateMap<EventDto, EventViewmodel>();
            cfg.CreateMap<EventDateDto, EventDateViewmodel>();
            cfg.CreateMap<EventDateUserDto, EventDateUserViewmodel>();
            cfg.CreateMap<EventStepDto, EventStepViewmodel>();
            cfg.CreateMap<EventStepDto, EventStepViewmodel>();
            cfg.CreateMap<EventStepUserDto, EventStepUserViewmodel>();
        });
    }
}
