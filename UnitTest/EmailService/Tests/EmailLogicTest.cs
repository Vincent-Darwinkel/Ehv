using Email_Service.Logic;
using Email_Service.Models.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using UnitTest.EmailService.TestModels.Helpers;

namespace UnitTest.EmailService
{
    [TestFixture]
    public class EmailLogicTest
    {
        private readonly EmailLogic _emailLogic;

        public EmailLogicTest()
        {
            _emailLogic = new EmailLogic(null);
        }

        [Test]
        public void SendMailsArgumentNullExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(() => _emailLogic.SendMails(null));
            Assert.Throws<ArgumentNullException>(() => _emailLogic.SendMails(new List<Email>()));
        }

        [Test]
        public void SendMailsNoNullAllowedException()
        {
            var testEmails = new TestEmail();
            Assert.Throws<NoNullAllowedException>(() => _emailLogic.SendMails(new List<Email>
            {
                testEmails.TestEmailEmptyTemplate
            }));
        }
    }
}