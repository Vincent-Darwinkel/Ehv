using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Datepicker_Service.Models.FromFrontend
{
    public class UserAvailability
    {
        [Required]
        public List<Guid> AvailableDates { get; set; }
        [Required]
        public Guid DatepickerUuid { get; set; }
    }
}