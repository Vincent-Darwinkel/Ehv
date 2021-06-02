namespace Account_Removal_Service.Models.Helpers
{
    public static class RabbitMqExchange
    {
        public static readonly string EventExchange = "event_exchange";
        public static readonly string DatepickerExchange = "datepicker_exchange";
        public static readonly string FileExchange = "file_exchange";
        public static readonly string AuthorizationExchange = "auth_exchange";
        public static readonly string UserExchange = "user_exchange";
        public static readonly string LogExchange = "log_exchange";
    }
}