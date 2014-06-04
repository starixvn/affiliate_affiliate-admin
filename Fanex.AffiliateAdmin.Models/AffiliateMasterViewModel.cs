// -----------------------------------------------------------------------
// <copyright file="AffiliateMasterViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateMasterViewModel
    {
        public Uri SiteDomainUrl { get; set; }

        public string UserName { get; set; }

        public Uri GetUrl(string controllerName, string actionName)
        {
            if (string.IsNullOrEmpty(this.UserName))
            {
                return new Uri(string.Format("{0}{1}/{2}", this.SiteDomainUrl, controllerName, actionName));
            }

            return new Uri(string.Format("{0}{1}/{2}?UserName={3}", this.SiteDomainUrl, controllerName, actionName, this.UserName));
        }
    }
}
