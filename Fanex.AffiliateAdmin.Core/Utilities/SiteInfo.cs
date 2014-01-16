// --------------------------------------------------------------------------
// <copyright file="SiteInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.1.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteInfo
    {
        public string AffAdminDomain { get; set; }

        public string AffiliateDomain { get; set; }

        public string MemberSiteDomain { get; set; }

        /// <summary>
        /// Parse info from request.
        /// </summary>
        /// <param name="request">HTTP request.</param>
        /// <param name="isKiosk">Whether it is kiosk or not.</param>
        /// <returns>Return site information.</returns>
        public static SiteInfo Parse(HttpRequestBase request, bool isKiosk)
        {
            var siteUrl = request.Url;

            var regex = new Regex(@":\d+");
            var host = regex.Replace(siteUrl.Authority, string.Empty);
            var scheme = siteUrl.Scheme;

            var affAdminDomain = host;
            var affiliateDomain = host;
            var memberSiteDomain = host;

            if (AppSettings.Environment.Equals("Production") && !string.IsNullOrEmpty(host))
            {
                var dotFirstPosition = host.IndexOf('.');

                if (dotFirstPosition > 0)
                {
                    memberSiteDomain = host.Substring(dotFirstPosition + 1, host.Length - dotFirstPosition - 1);
                    affiliateDomain = string.Format("{0}.{1}", isKiosk ? "agt" : "affiliate", memberSiteDomain);

                    return new SiteInfo
                    {
                        AffAdminDomain = string.Format("{0}://{1}", scheme, affAdminDomain),
                        AffiliateDomain = string.Format("{0}://{1}", scheme, affiliateDomain),
                        MemberSiteDomain = string.Format("{0}://www.{1}/vender.aspx", scheme, memberSiteDomain)
                    };
                }
            }

            return new SiteInfo
            {
                AffiliateDomain = isKiosk ? AppSettings.InternalURLKiosk : AppSettings.InternalURL
            };
        }
    }
}
