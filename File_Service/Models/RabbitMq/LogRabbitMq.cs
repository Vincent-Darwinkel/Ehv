using File_Service.Enums;

namespace File_Service.Models.RabbitMq
{
    public class LogRabbitMq
    {
        public readonly string FromMicroService = "File_Service";
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public LogType LogType { get; set; } = LogType.Bug;
    }
}