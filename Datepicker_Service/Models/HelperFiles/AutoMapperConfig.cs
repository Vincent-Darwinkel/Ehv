using AutoMapper;
using Datepicker_Service.Models.FromFrontend;
using Datepicker_Service.Models.RabbitMq;
using Datepicker_Service.Models.ToFrontend;

namespace Datepicker_Service.Models.HelperFiles
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Datepicker, DatepickerDto>();
                cfg.CreateMap<DatepickerDate, DatepickerDateDto>();
                cfg.CreateMap<DatepickerAvailabilityViewmodel, DatepickerAvailabilityDto>();
                cfg.CreateMap<DatepickerDto, DatepickerViewmodel>();
                cfg.CreateMap<DatepickerDateDto, DatePickerDateViewmodel>();
                cfg.CreateMap<DatepickerAvailabilityDto, DatepickerAvailabilityViewmodel>();
                cfg.CreateMap<DatepickerRabbitMq, DatepickerDto>();
                cfg.CreateMap<DatepickerDateRabbitMq, DatepickerDateDto>();
                cfg.CreateMap<EventStepRabbitMq, EventStepRabbitMq>();
                cfg.CreateMap<DatepickerDto, DatepickerRabbitMq>();
                cfg.CreateMap<DatepickerDateDto, DatepickerDateRabbitMq>();
                cfg.CreateMap<DatepickerAvailabilityDto, DatepickerAvailabilityRabbitMq>();
                cfg.CreateMap<EventStep, EventStepRabbitMq>();
            });
    }
}
