using Email_Service.Models.Helpers;
using System.Collections.Generic;

namespace UnitTest.EmailService.TestModels.Helpers
{
    public class TestEmail
    {
        public readonly Email TestEmailEmpty = new Email();
        public readonly Email TestEmailEmptyMessage = new Email
        {
            EmailAddress = "test@example.com",
            Subject = "Test"
        };
        public readonly Email TestEmailEmptyAddress = new Email
        {
            Subject = "Test",
            Message = "Test"
        };
        public readonly Email TestEmailEmptySubject = new Email
        {
            EmailAddress = "test@example.com",
            Message = "Test"
        };
        public readonly Email TestEmailEmptyTemplate = new Email
        {
            EmailAddress = "test@example.com",
            Message = "Test",
            KeyWordValues = new List<EmailKeyWordValue>
            {
                new EmailKeyWordValue
                {
                    Key = "test",
                    Value = "test"
                }
            },
            Subject = "test"
        };
    }
}