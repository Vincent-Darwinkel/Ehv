using Logging_Service.Enums;
using Logging_Service.Models;
using System;

namespace UnitTest.LoggingService.TestModels
{
    public class TestLogDto
    {
        public readonly LogDto Log = new LogDto
        {
            DateTime = DateTime.Now.AddDays(5),
            FromMicroService = "Test_Microservice",
            Message = "Test message",
            LogType = LogType.Bug,
            Stacktrace = "Test stacktrace",
            Uuid = Guid.Parse("eac54c31-8e07-4e4f-bd52-6e59bd7ad223")
        };
    }
}