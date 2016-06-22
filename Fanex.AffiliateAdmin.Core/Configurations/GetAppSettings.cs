namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Get value of application settings.
    /// </summary>
    /// <typeparam name="T">A generic type.</typeparam>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public static class GetAppSettings<T>
    {
        public static T GetValue(string key)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
    }
}
