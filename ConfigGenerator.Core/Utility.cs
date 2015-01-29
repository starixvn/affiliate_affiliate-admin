namespace ConfigGenerator.Core
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Utility class.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Get model folder.
        /// </summary>
        /// <returns>Model folder.</returns>
        public static string GetModelFolder()
        {
            return ConfigurationManager.AppSettings["ModelFolder"];
        }

        /// <summary>
        /// Get configuration folder.
        /// </summary>
        /// <returns>Configuration folder.</returns>
        public static string GetConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["ConfigFolder"];
        }

        /// <summary>
        /// Get admin configuration folder.
        /// </summary>
        /// <returns>Configuration folder.</returns>
        public static string GetAdminConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["AdminConfigFolder"];
        }

        /// <summary>
        /// Get user configuration folder.
        /// </summary>
        /// <returns>Configuration folder.</returns>
        public static string GetUserConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["UserConfigFolder"];
        }

        /// <summary>
        /// Get banner configuration folder.
        /// </summary>
        /// <returns>Configuration folder.</returns>
        public static string GetBannerConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["BannerConfigFolder"];
        }

        /// <summary>
        /// Get link configuration folder.
        /// </summary>
        /// <returns>Configuration folder.</returns>
        public static string GetLinkConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["LinkConfigFolder"];
        }

        /// <summary>
        /// Get web service configuration folder.
        /// </summary>
        /// <returns>Configuration folder.</returns>
        public static string GetWebServiceConfigurationFolder()
        {
            return ConfigurationManager.AppSettings["WebServiceConfigFolder"];
        }
    }
}