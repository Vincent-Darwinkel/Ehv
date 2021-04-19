using System;
using System.Collections.Generic;

namespace Datepicker_Service.Models.ToFrontend
{
    public class DatePickerDate
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public Guid DatePickerUuid { get; set; }
        public DateTime DateTime { get; set; }
        public List<DatepickerAvailability> UserAvailabilities { get; set; } = new List<DatepickerAvailability>();
    }
}