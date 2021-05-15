using System;
using Logging_Service.Enums;

namespace Logging_Service.Models.RabbitMq
{
    public class LogRabbitMq
    {
        public string FromMicroService { get; set; }
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public LogType LogType { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}