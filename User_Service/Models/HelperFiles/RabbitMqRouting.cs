namespace User_Service.Models.HelperFiles
{
    public static class RabbitMqRouting
    {
        public static readonly string AddUser = "create.user";
        public static readonly string FindsUsers = "find.user";
        public static readonly string UpdateUser = "update.user";
        public static readonly string DeleteUser = "delete.user";
    }
}