using System;

namespace Datepicker_Service.Models
{
    public class DatepickerAvailabilityDto
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public Guid DateUuid { get; set; }
        public Guid UserUuid { get; set; }
    }
}