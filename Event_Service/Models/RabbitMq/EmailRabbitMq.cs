using System.Collections.Generic;
using Event_Service.Models.HelperFiles;

namespace Event_Service.Models.RabbitMq
{
    public class EmailRabbitMq
    {
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string TemplateName { get; set; }
        public List<EmailKeyWordValue> KeyWordValues { get; set; }
    }
}