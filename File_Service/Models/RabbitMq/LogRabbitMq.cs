﻿namespace File_Service.Models.RabbitMq
{
    public class LogRabbitMq
    {
        public readonly string FromMicroService = "File_Service";
        public string Message { get; set; }
        public string Stacktrace { get; set; }
    }
}