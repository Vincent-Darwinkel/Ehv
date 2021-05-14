using System;
using System.Collections.Generic;

namespace Datepicker_Service.Models.FromFrontend
{
    public class UserAvailability
    {
        public List<Guid> AvailableDates { get; set; }
        public Guid DatepickerUuid { get; set; }
    }
}