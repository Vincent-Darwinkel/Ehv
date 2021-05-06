namespace Email_Service.Models.Helpers
{
    public static class RabbitMqRouting
    {
        public static readonly string SendMail = "send.mail";
        public static readonly string FindUsers = "find.user";
        public static readonly string AddLog = "add.log";
    }
}