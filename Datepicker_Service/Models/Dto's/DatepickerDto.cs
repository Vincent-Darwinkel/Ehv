using System;
using System.Collections.Generic;

namespace Datepicker_Service.Models
{
    public class DatepickerDto
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public Guid AuthorUuid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Expires { get; set; }
        public List<DatepickerDateDto> Dates { get; set; } = new List<DatepickerDateDto>();
    }
}
