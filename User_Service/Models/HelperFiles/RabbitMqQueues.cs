namespace User_Service.Models.HelperFiles
{
    public static class RabbitMqQueues
    {
        public static readonly string FindUserQueue = "find_user_queue";
        public static readonly string AddActivationQueue = "add_activation_queue";
        public static readonly string DeleteUserQueue = "delete_user_queue";
        public static readonly string DisabledExistsUserQueue = "exists_disabled_queue";
        public static readonly string ValidateUserPasswordQueue = "validate_password_user_queue";
    }
}