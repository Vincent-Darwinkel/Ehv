using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Datepicker_Service.Models.FromFrontend
{
    public class Datepicker
    {
        public Guid Uuid { get; set; }
        [Required]
        public Guid AuthorUuid { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime Expires { get; set; }
        public List<DatepickerDate> Dates { get; set; }
    }
}