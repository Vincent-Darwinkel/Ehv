namespace Email_Service.Models.Helpers
{
    public static class RabbitMqQueues
    {
        public static readonly string FindUserQueue = "find_user_queue";
        public static readonly string MailQueue = "send_mail_queue";
    }
}