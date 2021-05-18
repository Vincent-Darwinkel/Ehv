using System;
using System.ComponentModel.DataAnnotations;

namespace Datepicker_Service.Models.FromFrontend
{
    public class DatepickerAvailability
    {
        [Required]
        public Guid DateUuid { get; set; }
        [Required]
        public Guid DatepickerUuid { get; set; }
    }
}