﻿using AutoMapper;
using Datepicker_Service.Models.FromFrontend;
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
            });
    }
}