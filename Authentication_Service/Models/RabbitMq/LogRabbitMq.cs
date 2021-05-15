using Authentication_Service.Enums;

namespace Authentication_Service.Models.RabbitMq
{
    public class LogRabbitMq
    {
        public readonly string FromMicroService = "Auth_Service";
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public LogType LogType { get; set; } = LogType.Bug;
    }
}