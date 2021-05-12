using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Email_Service.Logic;
using Email_Service.Models.Helpers;
using NUnit.Framework;

namespace Email_Service.UnitTests
{
    [TestFixture]
    public class EmailLogicTest
    {
        private readonly EmailLogic _emailLogic;

        public EmailLogicTest()
        {
            _emailLogic = new EmailLogic(null, null);
        }

        [Test]
        public void EmailTemplateGeneratorTest()
        {
            string templateName = "LoginMultiRole";
            string emailTemplate = _emailLogic.GetHtmlFormattedEmail(templateName, new List<EmailKeyWordValue>
            {
                new EmailKeyWordValue
                {
                    Key = "@{Username}",
                    Value = "Test"
                },
                new EmailKeyWordValue
                {
                    Key = "@{LoginCode}",
                    Value = "Test"
                }
            });

            Assert.IsTrue(emailTemplate.Contains("Test"));
        }

        [Test]
        public void EmailTemplateGeneratorFileNotFoundExceptionTest()
        {
            Assert.Throws<FileNotFoundException>(() => _emailLogic.GetHtmlFormattedEmail(null, null));
        }

        [Test]
        public void SendMailsArgumentNullExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(() => _emailLogic.SendMails(null));
            Assert.Throws<ArgumentNullException>(() => _emailLogic.SendMails(new List<Email>()));
        }
    }
}