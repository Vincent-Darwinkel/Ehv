using System.Collections.Generic;

namespace Email_Service.Models.Helpers
{
    public static class EmailTemplatePaths
    {
        public static string GetTemplatePathByName(string templateName)
        {
            var dictionary = new Dictionary<string, string>
            {
                { "LoginMultiRole", "/EmailTemplates/LoginMultiRole.html" },
                { "ActivateAccount", "/EmailTemplates/ActivateAccount.html" },
                { "DatepickerConversion", "/EmailTemplates/DatepickerConversion.html" }
            };

            return dictionary[templateName];
        }
    }
}