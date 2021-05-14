using System;

namespace Datepicker_Service.Models
{
    public class DatepickerAvailabilityDto
    {
        public Guid Uuid { get; set; }
        public Guid DateUuid { get; set; }
        public Guid UserUuid { get; set; }
    }
}