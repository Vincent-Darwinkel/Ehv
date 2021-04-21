using Email_Service.Models.Helpers;

namespace Email_Service.UnitTests.TestModels
{
    public class TestEmail
    {
        public readonly Email TestEmailEmpty = new Email { };
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
    }
}