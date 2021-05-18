using Authentication_Service.Logic;
using NUnit.Framework;
using System;
using System.Data;
using UnitTest.AuthenticationService.MockedLogics;

namespace UnitTest.AuthenticationService.Tests
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
            Assert.DoesNotThrow(() => _logLogic.Log(testException));
        }

        [Test]
        public void LogTestMessageContainsSensitiveData()
        {
            var testInnerException = new NoNullAllowedException();
            var testException = new Exception("test exception Password", testInnerException);
            Assert.DoesNotThrow(() => _logLogic.Log(testException));
        }
    }
}
