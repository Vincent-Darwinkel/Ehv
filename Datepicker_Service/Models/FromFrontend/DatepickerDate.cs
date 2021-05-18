using System;
using System.ComponentModel.DataAnnotations;

namespace Datepicker_Service.Models.FromFrontend
{
    public class DatepickerDate
    {
        public Guid Uuid { get; set; }
        public Guid DatePickerUuid { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
    }
}
