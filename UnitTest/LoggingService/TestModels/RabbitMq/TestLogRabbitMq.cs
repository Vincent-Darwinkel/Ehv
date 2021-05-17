using Logging_Service.Models.RabbitMq;

namespace UnitTest.LoggingService.TestModels.RabbitMq
{
    public class TestLogRabbitMq
    {
        public readonly LogRabbitMq Log = new LogRabbitMq
        {
            DateTime = new TestLogDto().Log.DateTime,
            FromMicroService = new TestLogDto().Log.FromMicroService,
            LogType = new TestLogDto().Log.LogType,
            Message = new TestLogDto().Log.Message,
            Stacktrace = new TestLogDto().Log.Stacktrace
        };
    }
}