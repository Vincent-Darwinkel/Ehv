namespace Email_Service.Models.Helpers
{
    public static class RabbitMqRouting
    {
        public static readonly string SendMail = "send.mail";
        public static readonly string FindUsers = "find.user";
    }
}