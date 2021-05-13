namespace Authentication_Service.Models.HelperFiles
{
    public static class RabbitMqRouting
    {
        public static readonly string AddLog = "add.log";
        public static readonly string AddUser = "create.user";
        public static readonly string UpdateUser = "update.user";
        public static readonly string DeleteUser = "delete.user";
        public static readonly string SendMail = "send.mail";
        public static readonly string FindUsers = "find.user";
    }
}