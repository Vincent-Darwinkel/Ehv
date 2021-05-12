using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Email_Service.Models.Helpers;
using Email_Service.Models.RabbitMq;
using Email_Service.RabbitMq.Rpc;
using Microsoft.Extensions.Configuration;

namespace Email_Service.Logic
{
    public class EmailLogic
    {
        private readonly IConfiguration _config;
        private readonly RpcClient _rpcClient;

        public EmailLogic(IConfiguration config, RpcClient rpcClient)
        {
            _config = config;
            _rpcClient = rpcClient;
        }

        /// <summary>
        /// Finds an HTML template by the templateName parameter and replaces all @{} values with the value in the keyValueCollection parameter
        /// </summary>
        /// <param name="templateName">The path of the template</param>
        /// <param name="keyValueCollection">The key in the template to search and the value to replace it with</param>
        /// <returns>An string which contains HTML content</returns>
        public string GetHtmlFormattedEmail(string templateName, List<EmailKeyWordValue> keyValueCollection)
        {
            string templatePath = EmailTemplatePaths.GetTemplatePathByName(templateName);
            string filePath = Environment.CurrentDirectory + templatePath;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(nameof(templateName));
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
        /// Sends all the emails and includes the email template if specified
        /// </summary>
        /// <param name="emails">The emails to send</param>
        public void SendMails(List<Email> emails)
        {
            if (emails == null || emails.Count == 0)
            {
                throw new ArgumentNullException(nameof(emails));
            }

            List<Guid> userUuidCollection = emails
                .Select(e => e.UserUuid)
                .ToList();

            var users = _rpcClient.Call<List<UserRabbitMq>>(userUuidCollection, RabbitMqQueues.FindUserQueue);
            /* foreach (var email in emails)
             {
                 email.EmailAddress = users
                     .Find(u => u.Uuid == email.UserUuid)
                     .Email;

                 if (!string.IsNullOrEmpty(email.TemplateName))
                 {
                     email.Message = GetHtmlFormattedEmail(email.TemplateName, email.KeyWordValues);
                 }

                 Send(email);
             }*/
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="email">The email to send</param>
        private void Send(Email email)
        {
            if (string.IsNullOrEmpty(email?.Message) ||
                string.IsNullOrEmpty(email.EmailAddress) ||
                string.IsNullOrEmpty(email.Subject))
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
