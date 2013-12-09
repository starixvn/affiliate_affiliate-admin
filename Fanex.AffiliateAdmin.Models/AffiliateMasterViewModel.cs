// -----------------------------------------------------------------------
// <copyright file="AffiliateMasterViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateMasterViewModel
    {
        public string SiteDomainUrl { get; set; }

        public string UserName { get; set; }

        public string GetUrl(string controllerName, string actionName)
        {
            if (string.IsNullOrEmpty(this.UserName))
            {
                return string.Format("{0}/{1}/{2}", this.SiteDomainUrl, controllerName, actionName);
            }

            return string.Format("{0}/{1}/{2}?UserName={3}", this.SiteDomainUrl, controllerName, actionName, this.UserName);
        }
    }
}
