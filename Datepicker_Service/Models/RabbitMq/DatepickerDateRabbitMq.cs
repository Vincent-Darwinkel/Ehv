using System;
using System.Collections.Generic;

namespace Datepicker_Service.Models.RabbitMq
{
    public class DatepickerDateRabbitMq
    {
        public Guid Uuid { get; set; }
        public Guid DatePickerUuid { get; set; }
        public DateTime DateTime { get; set; }
        public List<DatepickerAvailabilityRabbitMq> UserAvailabilities { get; set; }
    }
}