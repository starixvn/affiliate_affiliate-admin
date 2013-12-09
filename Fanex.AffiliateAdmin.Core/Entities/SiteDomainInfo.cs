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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteDomainInfo
    {
        public string MemberSiteDomain { get; set; }

        public string MemberSiteUrl { get; set; }

        public string AdminSiteUrl { get; set; }

        public string AffiliateSiteUrl { get; set; }

        /// <summary>
        /// Parse site domain information.
        /// </summary>
        /// <param name="controller">Controller name.</param>
        /// <returns>Return Site domain info.</returns>
        public static SiteDomainInfo Parse(Controller controller)
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

            var adminSiteDomain = host;
            var adminSitePath = controller.Url.Content("~/");
            if (adminSitePath.EndsWith("/"))
            {
                adminSitePath = adminSitePath.Remove(adminSitePath.Length - 1);
            }

            var adminSiteUrl = string.Format("{0}://{1}{2}", scheme, host, adminSitePath);
            var memberSiteDomain = host;
            var memberSiteUrl = adminSiteUrl;
            var affiliateSiteUrl = adminSiteUrl;
            
            if (siteUrl.HostNameType == UriHostNameType.Dns)
            {
                var words = host.Split('.');
                if (words.Length > 2)
                {
                    memberSiteDomain = string.Format("{0}.{1}", words[1], words[2]);
                    memberSiteUrl = string.Format("{0}://www.{1}.{2}", scheme, words[1], words[2]);
                    affiliateSiteUrl = string.Format("{0}://affiliate.{1}.{2}", scheme, words[1], words[2]);
                    adminSiteUrl = string.Format("{0}://affadmin.{1}.{2}", scheme, words[1], words[2]);
                }
            }

            return new SiteDomainInfo
            {
                MemberSiteDomain = memberSiteDomain,
                MemberSiteUrl = memberSiteUrl,
                AffiliateSiteUrl = affiliateSiteUrl,
                AdminSiteUrl = adminSiteUrl
            };
        }
    }
}