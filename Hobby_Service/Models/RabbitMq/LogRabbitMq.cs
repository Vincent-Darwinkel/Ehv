namespace Hobby_Service.Models.RabbitMq
{
    public class LogRabbitMq
    {
        public readonly string FromMicroService = "Hobby_Service";
        public string Message { get; set; }
        public string Stacktrace { get; set; }
    }
}