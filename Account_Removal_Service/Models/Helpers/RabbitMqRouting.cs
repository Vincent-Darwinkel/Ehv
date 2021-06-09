namespace Account_Removal_Service.Models.Helpers
{
    public static class RabbitMqRouting
    {
        public static readonly string RemoveUserFromEventService = "remove.user.event";
        public static readonly string RemoveUserFromDatepickerService = "remove.user.datepicker";
        public static readonly string RemoveUserFromFileService = "remove.user.file";
        public static readonly string RemoveUserFromAuthorizationService = "remove.user.authorization";
        public static readonly string RemoveUserFromUserService = "remove.user.user";
        public static readonly string AddLog = "add.log";
    }
}
