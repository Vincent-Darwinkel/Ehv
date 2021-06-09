namespace User_Service.Models.HelperFiles
{
    public static class RabbitMqExchange
    {
        public static readonly string UserExchange = "user_exchange";
        public static readonly string AuthenticationExchange = "auth_exchange";
        public static readonly string LogExchange = "log_exchange";
        public static readonly string MailExchange = "mail_exchange";
    }
}