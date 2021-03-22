using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Email_Service.Logic;
using Email_Service.Models.Helpers;
using NUnit.Framework;

namespace Email_Service.Tests
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
        public void EmailTemplateGeneratorTest()
        {
            string emailTemplate = _emailLogic.GetHtmlFormattedEmail(EmailTemplatePaths.LoginMultiRole, new List<EmailKeyWordValue>
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

        [Test]
        public void SendMailNoNullAllowedExceptionTest()
        {
            Assert.Throws<NoNullAllowedException>(() => _emailLogic.Send(null));
            Assert.Throws<NoNullAllowedException>(() => _emailLogic.Send(new Email()));
            Assert.Throws<NoNullAllowedException>(() => _emailLogic.Send(new Email
            {
                Message = "Test"
            }));
            Assert.Throws<NoNullAllowedException>(() => _emailLogic.Send(new Email
            {
                Message = "Test",
                EmailAddress = "Test"
            }));
            Assert.Throws<NoNullAllowedException>(() => _emailLogic.Send(new Email
            {
                Subject = "Test"
            }));
        }
    }
}