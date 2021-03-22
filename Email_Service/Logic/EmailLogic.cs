using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Email_Service.Models.Helpers;
using Microsoft.Extensions.Configuration;

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
        /// Finds an HTML template by the templatePath parameter and replaces all @{} values with the value in the keyValueCollection parameter
        /// </summary>
        /// <param name="templatePath">The path of the template</param>
        /// <param name="keyValueCollection">The key in the template to search and the value to replace it with</param>
        /// <returns>An string which contains HTML content</returns>
        public string GetHtmlFormattedEmail(string templatePath, List<EmailKeyWordValue> keyValueCollection)
        {
            string filePath = Environment.CurrentDirectory + templatePath;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(nameof(templatePath));
            }

            string fileText = File.ReadAllText(filePath);
            var sb = new StringBuilder(fileText);
            keyValueCollection.ForEach(emailKeyWordValue =>
            {
                sb.Replace(emailKeyWordValue.Key, emailKeyWordValue.Value);
            });

            return sb.ToString();
        }

        /// <summary>
        /// Uses the Send method in a foreach loop, if one mail fails the return value would be false
        /// </summary>
        /// <param name="emails">The emails to send</param>
        /// <returns></returns>
        public void SendMails(List<Email> emails)
        {
            if (emails == null || emails.Count == 0)
            {
                throw new ArgumentNullException(nameof(emails));
            }
            emails.ForEach(Send);
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="email"></param>
        public void Send(Email email)
        {
            if (string.IsNullOrEmpty(email?.Message) || string.IsNullOrEmpty(email.EmailAddress) || string.IsNullOrEmpty(email.Subject))
            {
                throw new NoNullAllowedException(nameof(email));
            }

            // client settings
            using var client = new SmtpClient(_config[ConfigurationParameters.SmtpHost])
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_config[ConfigurationParameters.EmailToSendFrom], _config[ConfigurationParameters.EmailPassword]),
                Port = Convert.ToInt32(_config[ConfigurationParameters.SmtpPort]),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 10000
            };

            // mail settings
            using var message = new MailMessage { From = new MailAddress(_config[ConfigurationParameters.EmailToSendFrom]) };
            message.To.Add(email.EmailAddress);
            message.Body = email.Message;
            message.Subject = email.Subject;
            message.IsBodyHtml = true;
            client.Send(message);
        }
    }
}
