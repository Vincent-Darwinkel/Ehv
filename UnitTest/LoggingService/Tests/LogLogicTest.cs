using Logging_Service.CustomExceptions;
using Logging_Service.Logic;
using Logging_Service.Models;
using Logging_Service.Models.RabbitMq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UnitTest.LoggingService.MockedLogics;
using UnitTest.LoggingService.TestModels;
using UnitTest.LoggingService.TestModels.RabbitMq;

namespace UnitTest.LoggingService.Tests
{
    [TestFixture]
    public class LogLogicTest
    {
        private readonly LogLogic _logLogic;

        public LogLogicTest()
        {
            _logLogic = new MockedLogLogic().LogLogic;
        }

        [Test]
        public void LogTest()
        {
            var testInnerException = new NoNullAllowedException();
            var testException = new Exception("test exception", testInnerException);
            Assert.DoesNotThrowAsync(() => _logLogic.Log(testException));
        }

        [Test]
        public void LogTestMessageContainsSensitiveData()
        {
            var testInnerException = new NoNullAllowedException();
            var testException = new Exception("test exception Password", testInnerException);
            Assert.DoesNotThrowAsync(() => _logLogic.Log(testException));
        }

        [Test]
        public void AddRabbitMqTest()
        {
            var log = new TestLogRabbitMq().Log;
            Assert.DoesNotThrowAsync(() => _logLogic.Add(log));
        }

        [Test]
        public void AddRabbitMqUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _logLogic.Add(new LogRabbitMq()));
        }

        [Test]
        public void AllTest()
        {
            Assert.DoesNotThrowAsync(() => _logLogic.All());
        }

        [Test]
        public async Task AllResultTest()
        {
            List<LogDto> result = await _logLogic.All();
            Assert.IsTrue(result.Any());
        }

        [Test]
        public void DeleteTest()
        {
            var log = new TestLogDto().Log;
            Assert.DoesNotThrowAsync(() => _logLogic.Delete(new List<Guid> { log.Uuid }));
        }

        [Test]
        public void DeleteUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _logLogic.Delete(new List<Guid>()));
        }
    }
}
