using System;
using Logging_Service.Enums;

namespace Logging_Service.Models.ToFrontend
{
    public class LogViewmodel
    {
        public Guid Uuid { get; set; }
        public string FromMicroService { get; set; }
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public LogType LogType { get; set; }
        public DateTime DateTime { get; set; }
    }
}