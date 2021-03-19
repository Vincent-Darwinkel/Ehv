namespace Email_Service.Models.Helpers
{
    public static class ConfigParameters
    {
        public static string SmtpHost { get; set; }
        public static string SmtpPort { get; set; }
        public static string EmailToSendFrom { get; set; }
        public static string EmailPassword { get; set; }
    }
}