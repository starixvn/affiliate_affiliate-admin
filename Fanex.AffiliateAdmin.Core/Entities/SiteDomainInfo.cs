// --------------------------------------------------------------------------
// <copyright file="SiteDomainInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.08.22">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Entities
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteDomainInfo
    {
        public string MemberSiteDomain { get; set; }

        public Uri MemberSiteUrl { get; set; }

        public Uri AdminSiteUrl { get; set; }

        public Uri AffiliateSiteUrl { get; set; }

        /// <summary>
        /// Parse site domain information.
        /// </summary>
        /// <param name="controller">Controller name.</param>
        /// <param name="isKiosk">Is Kiosk Affiliate.</param>
        /// <returns>Return Site domain info.</returns>
        public static SiteDomainInfo Parse(Controller controller, bool isKiosk)
        {
            var siteUrl = controller.Request.Url;
            var scheme = siteUrl.Scheme;
            var host = siteUrl.Authority;
            if (siteUrl.HostNameType == UriHostNameType.Dns)
            {
                var words = host.Split('.');
                if (words.Length > 2)
                {
                    var regex = new Regex(@":\d+");
                    host = regex.Replace(siteUrl.Authority, string.Empty);
                }
            }

            var adminSitePath = controller.Url.Content("~/");

            if (adminSitePath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                adminSitePath = adminSitePath.Remove(adminSitePath.Length - 1);
            }

            var adminSiteUrl = string.Format("{0}://{1}{2}", scheme, host, adminSitePath);
            var memberSiteDomain = host;
            var memberSiteUrl = adminSiteUrl;

            if (siteUrl.HostNameType == UriHostNameType.Dns && AppSettings.Environment.Equals("Production"))
            {
                var words = host.Split('.');
                if (words.Length > 2)
                {
                    return new SiteDomainInfo
                    {
                        MemberSiteDomain = string.Format("www.{0}.{1}", words[1], words[2]),
                        MemberSiteUrl =  new Uri(string.Format("{0}://www.{1}.{2}/vender.aspx", scheme, words[1], words[2])),
                        AffiliateSiteUrl = new Uri(string.Format("{0}://{1}.{2}.{3}", scheme, isKiosk ? "agt" : "affiliate", words[1], words[2])),
                        AdminSiteUrl = new Uri(string.Format("{0}://{1}.{2}.{3}", scheme, "affadmin", words[1], words[2]))
                    };
                }
            }

            return new SiteDomainInfo
            {
                MemberSiteDomain = memberSiteDomain,
                MemberSiteUrl = new Uri(memberSiteUrl),
                AffiliateSiteUrl = isKiosk ? AppSettings.InternalUrlKiosk : AppSettings.InternalUrl,
                AdminSiteUrl = new Uri(adminSiteUrl)
            };
        }
    }
}