﻿namespace User_Service.Models.RabbitMq
{
    public class LogRabbitMq
    {
        public readonly string FromMicroService = "User_Service";
        public string Message { get; set; }
        public string Stacktrace { get; set; }
    }
}