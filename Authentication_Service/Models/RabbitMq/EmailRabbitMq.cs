using System;
using System.Collections.Generic;
using Authentication_Service.Models.HelperFiles;

namespace Authentication_Service.Models.RabbitMq
{
    public class EmailRabbitMq
    {
        public Guid UserUuid { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string TemplateName { get; set; }
        public List<EmailKeyWordValue> KeyWordValues { get; set; }
    }
}