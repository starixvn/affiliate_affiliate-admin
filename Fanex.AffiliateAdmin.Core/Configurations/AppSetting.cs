namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    public class AppSetting : IAppSetting
    {
        public static IAppSetting Instance => new AppSetting();

        public string DefaultLanguage
        {
            get
            {
                return GetValue("DefaultLanguage");
            }
        }

        public string Environment
        {
            get
            {
                return GetValue("Environment");
            }
        }

        public Uri InternalUrl
        {
            get
            {
                return new Uri(GetValue("InternalURL"));
            }
        }

        public Uri InternalUrlKiosk
        {
            get
            {
                return new Uri(GetValue("InternalURL_Kiosk"));
            }
        }

        public Uri SiteCheckerUrl
        {
            get
            {
                return new Uri(GetValue("SiteCheckerURL"));
            }
        }

        public Uri IPCheckerUrl
        {
            get
            {
                return new Uri(GetValue("IPCheckerURL"));
            }
        }

        public string ITMail
        {
            get
            {
                return GetValue("ITMail");
            }
        }

        public string Layout
        {
            get
            {
                return GetValue("Layout");
            }
        }

        public int LeftString
        {
            get
            {
                return Convert.ToInt32(GetValue("LeftString"));
            }
        }

        public bool LocalTest
        {
            get
            {
                return Environment.Equals("Local Development", StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool IsProduction
        {
            get
            {
                return Environment.Equals("Production");
            }
        }

        public string MailContact
        {
            get
            {
                return GetValue("MailContact");
            }
        }

        public decimal MinimumWithdrawalAmount
        {
            get
            {
                return Convert.ToDecimal(GetValue("MinimumWithdrawalAmount"));
            }
        }

        public int PageSize
        {
            get
            {
                return Convert.ToInt32(GetValue("PageSize"));
            }
        }

        public int ServerId
        {
            get
            {
                return Convert.ToInt32(GetValue("ServerID"));
            }
        }

        public string ServerTimeZone
        {
            get
            {
                var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                return string.Format("{0}{1:00}:00", offset.Hours > 0 ? "+" : string.Empty, offset.Hours);
            }
        }

        public string Theme
        {
            get
            {
                return GetValue("Theme");
            }
        }

        public int ViewLogBackMonth
        {
            get
            {
                return Convert.ToInt32(GetValue("ViewLogBackMonth"));
            }
        }

        public string DefaultCurrency
        {
            get
            {
                return GetValue("DefaultCurrency");
            }
        }

        public string BannerDomain
        {
            get
            {
                return GetValue("BannerDomain");
            }
        }

        public IEnumerable<string> BannerFolders
        {
            get
            {
                var value = GetValue("BannerFolders");
                return string.IsNullOrWhiteSpace(value) ? new string[] { } : value.Split(';');
            }
        }

        public long MaxImageSize
        {
            get
            {
                return Convert.ToInt64(GetValue("MaxImageSize"));
            }
        }

        public int ThumbnailHeight
        {
            get
            {
                return Convert.ToInt32(GetValue("ThumbnailHeight"));
            }
        }

        public int ThumbnailWidth
        {
            get
            {
                return Convert.ToInt32(GetValue("ThumbnailWidth"));
            }
        }

        /// <summary>
        /// Get key value of resource.
        /// </summary>
        /// <param name="key">Resource key.</param>
        /// <returns>Return value of resource key.</returns>
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
