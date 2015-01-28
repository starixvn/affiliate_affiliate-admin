using System.Configuration;

namespace ConfigGenerator.Core
{
    public static class Utility
    {
        public static string GetModelFolder()
        {
            return ConfigurationManager.AppSettings["ModelFolder"];
        }

        public static string GetConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["ConfigFolder"];
        }

        public static string GetAdminConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["AdminConfigFolder"];
        }

        public static string GetUserConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["UserConfigFolder"];
        }

        public static string GetBannerConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["BannerConfigFolder"];
        }

        public static string GetLinkConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["LinkConfigFolder"];
        }

        public static string GetWebServiceConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["WebServiceConfigFolder"];
        }
    }
}