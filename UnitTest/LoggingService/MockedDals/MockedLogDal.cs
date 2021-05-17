using System;
using System.Collections.Generic;
using Logging_Service.Dal.Interfaces;
using Logging_Service.Models;
using Moq;
using UnitTest.LoggingService.TestModels;

namespace UnitTest.LoggingService.MockedDals
{
    public class MockedLogDal
    {
        public readonly ILogDal Mock;

        public MockedLogDal()
        {
            var log = new TestLogDto().Log;
            var logDal = new Mock<ILogDal>();
            logDal.Setup(ld => ld.All()).ReturnsAsync(new List<LogDto> { log });

            Mock = logDal.Object;
        }
    }
}