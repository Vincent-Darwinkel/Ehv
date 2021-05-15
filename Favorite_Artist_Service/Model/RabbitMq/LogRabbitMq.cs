using Favorite_Artist_Service.Enums;

namespace Favorite_Artist_Service.Model.RabbitMq
{
    public class LogRabbitMq
    {
        public readonly string FromMicroService = "Favorite_Artist_Service";
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public LogType LogType { get; set; } = LogType.Bug;
    }
}