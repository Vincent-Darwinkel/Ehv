using System;

namespace Logging_Service.Models.RabbitMq
{
    public class LogRabbitMq
    {
        public string FromMicroService { get; set; }
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}