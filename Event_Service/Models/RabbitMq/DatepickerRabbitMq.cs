using System;
using System.Collections.Generic;

namespace Event_Service.Models.RabbitMq
{
    public class DatepickerRabbitMq
    {
        public Guid Uuid { get; set; }
        public Guid AuthorUuid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Expires { get; set; }
        public List<DatepickerDateRabbitMq> Dates { get; set; }
        public List<Guid> SelectedDates { get; set; }
        public List<EventStepRabbitMq> EventSteps { get; set; }
    }
}
