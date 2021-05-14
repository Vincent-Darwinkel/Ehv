using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Datepicker_Service.Models.FromFrontend
{
    public class DatePickerConversion
    {
        public Guid DatepickerUuid { get; set; }
        [Required]
        public List<Guid> SelectedDates { get; set; }
        public List<EventStep> EventSteps { get; set; }
    }
}