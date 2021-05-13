namespace Email_Service.Models.Helpers
{
    public class EmailConfig
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
    }
}