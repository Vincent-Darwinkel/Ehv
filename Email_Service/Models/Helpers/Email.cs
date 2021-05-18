using System.Collections.Generic;

namespace Email_Service.Models.Helpers
{
    public class Email
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string EmailAddress { get; set; }
        public string TemplateName { get; set; }
        public List<EmailKeyWordValue> KeyWordValues { get; set; }
    }
}
