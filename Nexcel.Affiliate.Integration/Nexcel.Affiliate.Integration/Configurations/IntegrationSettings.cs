using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;

namespace Nexcel.Affiliate.Integration.Configurations
{
    public class IntegrationSettings : ConfigurationSection, IIntegrationSettings
    {
        private static IntegrationSettings settings = ConfigurationManager.GetSection("Nexcel.Affiliate.Integration") as IntegrationSettings;
        private static readonly Regex UrlRegularExpression = new Regex("(http|https):\\/\\/([\\w@][\\w.:@]+)\\/?[\\w\\.?=%&=\\-@/$,]*");
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
                if (object.ReferenceEquals(IntegrationSettings.settings["PublicKeyPath"], null) || string.IsNullOrEmpty(IntegrationSettings.settings["PublicKeyPath"].ToString()))
                {
                    throw new ConfigurationErrorsException(string.Format(Resources.ConfigurationRequired, "PublicKeyPath"));
                }
                string text = IntegrationSettings.settings["PublicKeyPath"].ToString();
                if (text.StartsWith("~/"))
                {
                    if (HttpContext.Current == null)
                    {
                        throw new NullReferenceException(Resources.HttpContextNullException);
                    }
                    text = HttpContext.Current.Server.MapPath(text);
                }
                return text;
            }
            set
            {
                IntegrationSettings.settings["PublicKeyPath"] = value;
            }
        }
        [ConfigurationProperty("SsoUrl", IsRequired = true)]
        public string SsoUrl
        {
            get
            {
                if (object.ReferenceEquals(IntegrationSettings.settings["SsoUrl"], null) || string.IsNullOrEmpty(IntegrationSettings.settings["SsoUrl"].ToString()))
                {
                    throw new ConfigurationErrorsException(string.Format(Resources.ConfigurationRequired, "SsoUrl"));
                }
                string text = IntegrationSettings.settings["SsoUrl"].ToString();
                if (!IntegrationSettings.UrlRegularExpression.IsMatch(text))
                {
                    throw new Exception(string.Format(Resources.InvalidUrlFormat, "SsoUrl"));
                }
                return text;
            }
            set
            {
                IntegrationSettings.settings["SsoUrl"] = value;
            }
        }
        [ConfigurationProperty("InternalLoginUrl", IsRequired = true)]
        public string InternalLoginUrl
        {
            get
            {
                if (object.ReferenceEquals(IntegrationSettings.settings["InternalLoginUrl"], null) || string.IsNullOrEmpty(IntegrationSettings.settings["InternalLoginUrl"].ToString()))
                {
                    throw new ConfigurationErrorsException(string.Format(Resources.ConfigurationRequired, "InternalLoginUrl"));
                }
                string text = IntegrationSettings.settings["InternalLoginUrl"].ToString();
                if (!IntegrationSettings.UrlRegularExpression.IsMatch(text))
                {
                    throw new Exception(string.Format(Resources.InvalidUrlFormat, "InternalLoginUrl"));
                }
                return text;
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
                if (object.ReferenceEquals(IntegrationSettings.settings["LicenseeName"], null) || string.IsNullOrEmpty(IntegrationSettings.settings["LicenseeName"].ToString()))
                {
                    throw new ConfigurationErrorsException(string.Format(Resources.ConfigurationRequired, "LicenseeName"));
                }
                return IntegrationSettings.settings["LicenseeName"].ToString();
            }
            set
            {
                IntegrationSettings.settings["LicenseeName"] = value;
            }
        }
       

    }
}
