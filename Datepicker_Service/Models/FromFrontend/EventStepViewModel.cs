using System;
using System.ComponentModel.DataAnnotations;

namespace Datepicker_Service.Models.FromFrontend
{
    public class EventStepViewModel
    {
        public Guid Uuid { get; set; }
        [Required]
        public int StepNr { get; set; }
        [Required]
        public Guid EventUuid { get; set; }
        [Required]
        public string Text { get; set; }
    }
}