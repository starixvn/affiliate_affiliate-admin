namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class UrlUtil
    {
        /// <summary>
        /// Get base url from the request object.
        /// </summary>
        /// <param name="controller">Controller input.</param>
        /// <returns>Base Url Result.</returns>
        public static Uri GetBaseUrl(Controller controller)
        {
            var request = controller.Request;

            if (request == null || request.Url == null)
            {
                return null;
            }

            var subDomainPath = controller.Url.Content("~/");

            if (subDomainPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                subDomainPath = subDomainPath.Remove(subDomainPath.Length - 1);
            }

            return new Uri(request.Url.Scheme + Uri.SchemeDelimiter + request.Url.Host
                + ((request.Url.IsDefaultPort || !AppSetting.Instance.LocalTest)
                ? string.Empty : ":" + request.Url.Port) + subDomainPath);
        }
    }
}