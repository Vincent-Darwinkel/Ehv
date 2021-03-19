using System.Collections.Generic;
using Email_Service.Logic;
using Email_Service.Models.Helpers;
using Models.Helpers;
using NUnit.Framework;

namespace Email_Service.UnitTests
{
    [TestFixture]
    public class EmailLogicTest
    {
        private readonly EmailLogic _emailLogic = new EmailLogic(null);

        /// <summary>
        /// Test if the template generator replaces the specified words
        /// </summary>
        [Test]
        public void GetHtmlTemplateTest()
        {
            string template = _emailLogic.GetHtmlFormattedEmail(EmailTemplatePaths.LoginMultiRole, new List<EmailKeyWordValue>
            {
                new EmailKeyWordValue
                {
                    Key = "@Username",
                    Value = "Test"
                },
                new EmailKeyWordValue
                {
                    Key = "@LoginCode",
                    Value = "Test"
                }
            });

            Assert.That(template.Contains("Test"));
        }
    }
}