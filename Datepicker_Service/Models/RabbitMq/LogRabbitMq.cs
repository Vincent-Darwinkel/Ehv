using Datepicker_Service.Enums;

namespace Datepicker_Service.Models.RabbitMq
{
    public class LogRabbitMq
    {
        public readonly string FromMicroService = "Datepicker_Service";
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public LogType LogType { get; set; } = LogType.Bug;
    }
}