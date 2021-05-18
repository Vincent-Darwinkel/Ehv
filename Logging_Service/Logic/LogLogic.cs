using AutoMapper;
using Logging_Service.CustomExceptions;
using Logging_Service.Dal.Interfaces;
using Logging_Service.Models;
using Logging_Service.Models.RabbitMq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging_Service.Logic
{
    public class LogLogic
    {
        private readonly ILogDal _logDal;
        private readonly IMapper _mapper;
        private readonly string[] _sensitiveExceptionKeywords = { "password", "username", "salt", "hash", "email" };

        public LogLogic(ILogDal logDal, IMapper mapper)
        {
            _logDal = logDal;
            _mapper = mapper;
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
                if (!string.IsNullOrEmpty(exception.Message) && exception.Message.ToLower()
                    .Contains(sensitiveExceptionKeyword))
                {
                    return true;
                }
                if (!string.IsNullOrEmpty(exception.StackTrace) && exception.StackTrace.ToLower()
                    .Contains(sensitiveExceptionKeyword))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task Log(object e)
        {
            Exception exception = (Exception)e;
            if (ExceptionContainsSensitiveInformation(exception))
            {
                return;
            }

            await _logDal.Add(new LogDto
            {
                DateTime = DateTime.Now,
                FromMicroService = "Log_Service",
                Message = exception.Message,
                Stacktrace = exception.StackTrace,
                Uuid = Guid.NewGuid()
            });
        }

        private bool LogValid(LogRabbitMq log)
        {
            return !string.IsNullOrEmpty(log.Message) &&
                   !string.IsNullOrEmpty(log.Stacktrace) &&
                   !string.IsNullOrEmpty(log.FromMicroService);
        }

        public async Task Add(LogRabbitMq rabbitMqLog)
        {
            if (!LogValid(rabbitMqLog))
            {
                throw new UnprocessableException();
            }

            var log = _mapper.Map<LogDto>(rabbitMqLog);
            log.Uuid = Guid.NewGuid();
            await _logDal.Add(log);
        }

        public async Task<List<LogDto>> All()
        {
            return await _logDal.All();
        }

        public async Task Delete(List<Guid> uuidCollection)
        {
            if (!uuidCollection.Any())
            {
                throw new UnprocessableException();
            }

            await _logDal.Delete(uuidCollection);
        }
    }
}