namespace Fanex.AffiliateAdmin.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteDomainInfo
    {
        private static readonly IDictionary<string, string> _mappingBpadmin999Domains = new Dictionary<string, string>
        {
            { "itc303", "itcbetok.com" },
            { "macaubet", "resomer.my" },
            { "aoncash", "aoncashh88.net" },
            { "9bet", "9bet1.com" },
        };

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
            var host = siteUrl.Host;

            var adminSiteUrl = UrlUtil.GetBaseUrl(controller);
            var memberSiteDomain = host;
            var memberSiteUrl = adminSiteUrl;

            if (AppSettings.Environment.Equals("Production"))
            {
                var words = host.Split('.');
                if (words.Length > 2)
                {
                    if(words.Length > 3 && string.Equals("bpadmin999", words[2], StringComparison.CurrentCultureIgnoreCase))
                    {
                        if(_mappingBpadmin999Domains.TryGetValue(words[1].ToLowerInvariant(), out string memberDomain))
                        {
                            var items = memberDomain.Split('.');

                            return GetSiteDomainInfo(scheme, isKiosk, items[0], items[1]);
                        }

                        return GetSiteDomainInfo(scheme, isKiosk, words[1], words[3]);
                    }

                    if(string.Equals("starixplay", words[1], StringComparison.CurrentCultureIgnoreCase))
                    {
                        return new SiteDomainInfo
                        {
                            MemberSiteDomain = string.Format("www.{0}.{1}", AppSettings.SystemName, words[2]),
                            MemberSiteUrl = new Uri(string.Format("{0}://www.{1}.{2}/vender.aspx", scheme, AppSettings.SystemName, words[2])),
                            AffiliateSiteUrl = new Uri(string.Format("{0}://{1}.{2}.{3}", scheme, words[0].Replace("admin", "user"), words[1], words[2])),
                            AdminSiteUrl = new Uri(string.Format("{0}://{1}.{2}.{3}", scheme, words[0], words[1], words[2]))
                        };
                    }

                    return GetSiteDomainInfo(scheme, isKiosk, words[1], words[2]);
                }
            }

            return new SiteDomainInfo
            {
                MemberSiteDomain = memberSiteDomain,
                MemberSiteUrl = memberSiteUrl,
                AffiliateSiteUrl = isKiosk ? AppSettings.InternalUrlKiosk : AppSettings.InternalUrl,
                AdminSiteUrl = adminSiteUrl
            };
        }

        private static SiteDomainInfo GetSiteDomainInfo(string scheme, bool isKiosk, string arg1, string arg2)
        {
            return new SiteDomainInfo
            {
                MemberSiteDomain = string.Format("www.{0}.{1}", arg1, arg2),
                MemberSiteUrl = new Uri(string.Format("{0}://www.{1}.{2}/vender.aspx", scheme, arg1, arg2)),
                AffiliateSiteUrl = new Uri(string.Format("{0}://{1}.{2}.{3}", scheme, isKiosk ? "agt" : "affiliate", arg1, arg2)),
                AdminSiteUrl = new Uri(string.Format("{0}://{1}.{2}.{3}", scheme, "affadmin", arg1, arg2))
            };
        }
    }
}