using System;

namespace Datepicker_Service.Models.ToFrontend
{
    public class DatepickerAvailability
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public Guid DateUuid { get; set; }
        public Guid UserUuid { get; set; }
        public bool Available { get; set; }
        public string Username { get; set; }
    }
}