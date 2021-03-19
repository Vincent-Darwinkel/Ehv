using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Email_Service.Models.FromFrontend;
using Email_Service.Models.Helpers;
using Microsoft.Extensions.Configuration;
using Models.Helpers;

namespace Email_Service.Logic
{
    public class EmailLogic
    {
        private readonly IConfiguration _config;
        public EmailLogic(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Finds an html template by the templatePath parameter and replaces all @{} values with the value in the keyValueCollection parameter
        /// </summary>
        /// <param name="templatePath">The path of the template</param>
        /// <param name="keyValueCollection">The key in the template to search and the value to replace it with</param>
        /// <returns>An string which contains html content</returns>
        public string GetHtmlFormattedEmail(string templatePath, List<EmailKeyWordValue> keyValueCollection)
        {
            string filePath = Environment.CurrentDirectory + templatePath;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(nameof(templatePath));
            }

            string fileText = File.ReadAllText(filePath);
            var sb = new StringBuilder(fileText);
            keyValueCollection.ForEach(kv =>
            {
                string textToReplace = "@{" + kv.Key + "}";
                sb.Replace(textToReplace, kv.Value);
            });

            return sb.ToString();
        }

        /// <summary>
        /// Uses the Send method in a foreach loop, if one mail fails the return value would be false
        /// </summary>
        /// <param name="emails">List of email object which contains all data</param>
        public void SendMails(List<Email> emails)
        {
            if (emails == new List<Email>() || emails == null)
            {
                throw new ArgumentNullException(nameof(emails));
            }
            emails.ForEach(Send);
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="email">The email object which contains all data</param>
        public void Send(Email email)
        {
            if (string.IsNullOrEmpty(email?.Message) || string.IsNullOrEmpty(email.EmailAddress) || string.IsNullOrEmpty(email.Subject))
            {
                throw new NoNullAllowedException(nameof(email));
            }

            // client settings
            using var client = new SmtpClient(_config[ConfigParameters.SmtpHost])
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_config[ConfigParameters.EmailToSendFrom], _config[ConfigParameters.EmailPassword]),
                Port = Convert.ToInt32(_config[ConfigParameters.SmtpPort]),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 10000
            };

            // mail settings
            using var message = new MailMessage { From = new MailAddress(_config[ConfigParameters.EmailToSendFrom]) };
            message.To.Add(email.EmailAddress);
            message.Body = email.Message;
            message.Subject = email.Subject;
            message.IsBodyHtml = true;
            client.Send(message);
        }
    }
}
