﻿namespace Datepicker_Service.Models.HelperFiles
{
    public static class RabbitMqQueues
    {
        public static readonly string ExistsEventQueue = "exists_event_queue";
        public static readonly string LoggingQueue = "add_log_queue";
    }
}