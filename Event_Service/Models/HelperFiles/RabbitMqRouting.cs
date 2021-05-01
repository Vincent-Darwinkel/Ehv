namespace Event_Service.Models.HelperFiles
{
    public static class RabbitMqRouting
    {
        public static readonly string EventExists = "exists.event";
        public static readonly string AddLog = "add.log";
    }
}