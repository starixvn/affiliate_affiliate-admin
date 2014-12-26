// --------------------------------------------------------------------------
// <copyright file="UrlUtil.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.12.25">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Web.Mvc;

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
                + (request.Url.IsDefaultPort ? string.Empty : ":" + request.Url.Port) + subDomainPath);
        }
    }
}
