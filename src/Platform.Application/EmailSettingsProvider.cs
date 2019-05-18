using System.Collections.Generic;
using Abp.Configuration;

namespace Platform
{
    public class EmailSettingsProvider:SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(
                    "Abp.Net.Mail.Smtp.Host",
                    "smtp.gmail.com"),
                new SettingDefinition(
                    "Abp.Net.Mail.Smtp.Port",
                    "465"),
                new SettingDefinition("Abp.Net.Mail.Smtp.UserName",
                    "info@choizy.org"),
                new SettingDefinition("Abp.Net.Mail.Smtp.Password",
                    "kDGsgs19sdf"),
                new SettingDefinition("Abp.Net.Mail.Smtp.EnableSsl",
                    "true"),
                new SettingDefinition("Abp.Net.Mail.Smtp.UseDefaultCredentials",
                    "false"),
                new SettingDefinition(
                    "Abp.Net.Mail.Smtp.Domain",
                    "choizy.org"), 
            };
        }
    }
}