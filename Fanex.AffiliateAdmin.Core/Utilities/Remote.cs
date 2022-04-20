// -----------------------------------------------------------------------
// <copyright file="Remote.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public static class Remote
    {
        /// <summary>
        /// Contains all operating system mapping.
        /// </summary>
        private static IDictionary<string, string> _operatingSystemMapping = new Dictionary<string, string>
        {
                { "Windows NT 6.1", "Windows 7" },
                { "Windows NT 6.0", "Windows Vista" },
                { "Windows NT 5.1", "Windows XP" },
                { "Windows NT 5.0", "Windows 2000" },
                { "Windows NT 4.1", "Windows 98" },
                { "iPhone", "iOS" },
                { "iPad", "iOS" },
                { "iPod", "iOS" },
                { "Mac OS", "Mac OS" },
                { "BlackBerry", "BlackBerry" },
                { "Android", "Android" },
                { "Linux", "Linux" },
                { "Ubuntu", "Ubuntu" }
        };

        /// <summary>
        /// Contains all device mapping.
        /// </summary>
        private static IDictionary<string, string> _deviceMapping = new Dictionary<string, string>
        {
                { "iPhone", "iPhone" },
                { "iPad", "iPad" },
                { "iPod", "iPod" },
                { "BlackBerry", "BlackBerry" },
                { "HTC", "HTC" },
                { "Motorola", "Motorola" },
                { "Nokia", "Nokia" }
        };

        public static string GetClientIPAddress(HttpRequestBase httpRequest = null)
        {
            var request = httpRequest ?? new HttpRequestWrapper(HttpContext.Current.Request);
            string ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        /// Get IP value by long.
        /// </summary>
        /// <returns>IP value by long.</returns>
        public static long GetIPValue()
        {
            var ip = GetClientIPAddress();

            if (string.IsNullOrEmpty(ip) || ip == "127.0.0.1")
            {
                return 0;
            }

            var parts = ip.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            return (16777216 * int.Parse(parts[0]))
                    + (65536 * int.Parse(parts[1]))
                    + (256 * int.Parse(parts[2]))
                    + int.Parse(parts[3]);
        }

        /// <summary>
        /// Get query key word.
        /// </summary>
        /// <param name="urlReferrer">Referrer uri.</param>
        /// <returns>The key word.</returns>
        public static string GetKeyword(Uri urlReferrer)
        {
            if (urlReferrer != null)
            {
                var parameters = HttpUtility.ParseQueryString(urlReferrer.Query);
                if (urlReferrer.Host.Contains("google.com") ||
                    urlReferrer.Host.Contains("bing.com") ||
                    urlReferrer.Host.Contains("ask.com"))
                {
                    return parameters["q"];
                }
                else if (urlReferrer.Host.Contains("yahoo.com"))
                {
                    return parameters["p"];
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the region info of current request.
        /// </summary>
        /// <returns>Region Info.</returns>
        public static RegionInfo ResolveCountry()
        {
            var languages = HttpContext.Current.Request.UserLanguages;
            if (languages != null && languages.Length > 0)
            {
                var language = languages[0].ToLowerInvariant().Trim();
                try
                {
                    var culture = CultureInfo.CreateSpecificCulture(language);
                    return new RegionInfo(culture.LCID);
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Get request device name.
        /// </summary>
        /// <param name="userAgent">User agent.</param>
        /// <returns>Request device name.</returns>
        public static string GetDeviceName(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return @"Computer/Laptop";
            }

            foreach (var key in _deviceMapping.Keys)
            {
                if (userAgent.Contains(key))
                {
                    return _deviceMapping[key];
                }
            }

            return @"Computer/Laptop";
        }

        /// <summary>
        /// Get request operating system name.
        /// </summary>
        /// <param name="userAgent">User agent.</param>
        /// <returns>Operating system name.</returns>
        public static string GetOSName(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return string.Empty;
            }

            foreach (var key in _operatingSystemMapping.Keys)
            {
                if (userAgent.Contains(key))
                {
                    return _operatingSystemMapping[key];
                }
            }

            return string.Empty;
        }
    }
}