using Authentication_Service.Logic;
using NUnit.Framework;
using System;

namespace Authentication_Service.UnitTests
{
    [TestFixture]
    public class SecurityLogicTest
    {
        private readonly SecurityLogic _securityLogic = new SecurityLogic();

        [Test]
        public void HashPasswordTest()
        {
            const string password = "123";
            string hash = _securityLogic.HashPassword(password);
            Assert.False(string.IsNullOrEmpty(hash));
        }

        [Test]
        public void HashPasswordArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _securityLogic.HashPassword(null));
        }

        [Test]
        public void VerifyPasswordTest()
        {
            const string password = "123";
            string hash = _securityLogic.HashPassword(password);
            bool passwordValid = _securityLogic.VerifyPassword(password, hash);
            Assert.True(passwordValid);
        }

        [Test]
        public void VerifyPasswordArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _securityLogic.VerifyPassword(null, null));
            Assert.Throws(typeof(ArgumentNullException), () => _securityLogic.VerifyPassword("123", null));
            Assert.Throws(typeof(ArgumentNullException), () => _securityLogic.VerifyPassword(null, "123"));
            Assert.Throws(typeof(ArgumentNullException), () => _securityLogic.VerifyPassword("", ""));
        }
    }
}
