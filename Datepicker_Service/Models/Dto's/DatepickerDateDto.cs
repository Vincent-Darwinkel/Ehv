using System;
using System.Collections.Generic;

namespace Datepicker_Service.Models
{
    public class DatepickerDateDto
    {
        public Guid Uuid { get; set; }
        public Guid DatePickerUuid { get; set; }
        public DateTime DateTime { get; set; }
        public List<DatepickerAvailabilityDto> UserAvailabilities { get; set; } = new List<DatepickerAvailabilityDto>();
    }
}