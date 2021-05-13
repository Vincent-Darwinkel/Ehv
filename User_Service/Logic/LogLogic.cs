using System;
using RabbitMQ.Client;
using User_Service.Models.HelperFiles;
using User_Service.Models.RabbitMq;
using User_Service.RabbitMq;
using User_Service.RabbitMq.Publishers;

namespace User_Service.Logic
{
    public class LogLogic
    {
        private readonly IModel _channel;
        private readonly string[] _sensitiveExceptionKeywords = { "password", "username", "salt", "hash", "email" };

        public LogLogic(IModel channel)
        {
            _channel = channel;
        }

        /// <summary>
        /// Checks if the message or stacktrace of an exception contains sensitive data
        /// </summary>
        /// <param name="exception">The exception to check</param>
        /// <returns>True if sensitive data is included in the exception false if not</returns>
        private bool ExceptionContainsSensitiveInformation(Exception exception)
        {
            foreach (var sensitiveExceptionKeyword in _sensitiveExceptionKeywords)
            {
                if (exception.Message.Contains(sensitiveExceptionKeyword))
                {
                    return true;
                }
                if (!string.IsNullOrEmpty(exception.StackTrace) && exception.StackTrace.Contains(sensitiveExceptionKeyword))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Publishes the exception on the rabbit mq exchange
        /// </summary>
        /// <param name="e">The exception</param>
        public void Log(object e)
        {
            Exception exception = (Exception)e;
            if (ExceptionContainsSensitiveInformation(exception))
            {
                return;
            }

            var rabbitMqPublisher = new Publisher(_channel);
            rabbitMqPublisher.Publish(new LogRabbitMq
            {
                Message = exception.Message,
                Stacktrace = exception.StackTrace
            }, RabbitMqRouting.AddLog, RabbitMqExchange.LogExchange);
        }
    }
}