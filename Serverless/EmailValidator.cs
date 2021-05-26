using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Company.Function
{
    public static class EmailValidator
    {
        private static async Task<List<string>> GetTemporaryEmailProvidersByUrl(string[] urls)
        {
            var results = new List<string>();

            var client = new HttpClient();
            foreach (var url in urls)
            {
                string result = await client.GetStringAsync(url);
                string[] disposableMailProviders = result.Split(Environment.NewLine);
                results.AddRange(disposableMailProviders);
            }

            return results;
        }

        public static async Task<bool> EmailIsValid(string address)
        {
            if (string.IsNullOrEmpty(address) || !new EmailAddressAttribute().IsValid(address))
            {
                return false;
            }

            List<string> temporaryEmailProviders = await GetTemporaryEmailProvidersByUrl(new[]
            {
                "https://gist.githubusercontent.com/adamloving/4401361/raw/e81212c3caecb54b87ced6392e0a0de2b6466287/temporary-email-address-domains",
                "https://gist.githubusercontent.com/michenriksen/8710649/raw/e09ee253960ec1ff0add4f92b62616ebbe24ab87/disposable-email-provider-domains"
            });

            return !temporaryEmailProviders.Any(dmp => dmp.Contains(address));
        }

        [FunctionName("EmailValidator")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string email = req.Query["email"];
            bool emailValid = await EmailIsValid(email);

            return new OkObjectResult(emailValid);
        }
    }
}