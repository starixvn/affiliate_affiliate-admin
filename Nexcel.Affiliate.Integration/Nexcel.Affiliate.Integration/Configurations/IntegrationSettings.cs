namespace Nexcel.Affiliate.Integration.Configurations
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;
    using System.Web;

    public class IntegrationSettings : ConfigurationSection, IIntegrationSettings
    {
        private static IntegrationSettings settings = ConfigurationManager.GetSection("Nexcel.Affiliate.Integration") as IntegrationSettings;

        public static IntegrationSettings Settings
        {
            get
            {
                return IntegrationSettings.settings;
            }
        }

        [ConfigurationProperty("PublicKeyPath", IsRequired = true)]
        public string PublicKeyPath
        {
            get
            {
                string text = IntegrationSettings.settings["PublicKeyPath"].ToString();
                if (text.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                {
                    text = HttpContext.Current.Server.MapPath(text);
                }

                return text;
            }

            set
            {
                IntegrationSettings.settings["PublicKeyPath"] = value;
            }
        }

        [ConfigurationProperty("SsoUrl", IsRequired = false)]
        public Uri SsoUrl
        {
            get
            {
                return new Uri(IntegrationSettings.settings["SsoUrl"].ToString());
            }

            set
            {
                IntegrationSettings.settings["SsoUrl"] = value;
            }
        }

        [ConfigurationProperty("InternalLoginUrl", IsRequired = true)]
        public Uri InternalLoginUrl
        {
            get
            {
                return new Uri(IntegrationSettings.settings["InternalLoginUrl"].ToString());
            }

            set
            {
                IntegrationSettings.settings["InternalLoginUrl"] = value;
            }
        }

        [ConfigurationProperty("LicenseeName", IsRequired = true)]
        public string LicenseeName
        {
            get
            {
                return IntegrationSettings.settings["LicenseeName"].ToString();
            }

            set
            {
                IntegrationSettings.settings["LicenseeName"] = value;
            }
        }
    }
}