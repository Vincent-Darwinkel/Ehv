using System;
using Favorite_Artist_Service.Model.Helpers;
using Favorite_Artist_Service.Model.RabbitMq;
using Favorite_Artist_Service.RabbitMq.Publishers;

namespace Favorite_Artist_Service.Logic
{
    public class LogLogic
    {
        private readonly IPublisher _publisher;
        private readonly string[] _sensitiveExceptionKeywords = { "password", "username", "salt", "hash", "email" };

        public LogLogic(IPublisher publisher)
        {
            _publisher = publisher;
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
                if (exception.StackTrace.Contains(sensitiveExceptionKeyword))
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

            _publisher.Publish(new LogRabbitMq
            {
                Message = exception.Message,
                Stacktrace = exception.StackTrace
            }, RabbitMqRouting.AddLog, RabbitMqExchange.LogExchange);
        }
    }
}