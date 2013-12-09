// -----------------------------------------------------------------------
// <copyright file="AffiliateInfoViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateInfoViewModel
    {
        public AffiliateInfoViewModel()
        {
            OnUsers = new List<AffiliateOnUser>();
        }

        public IList<AffiliateOnUser> OnUsers { get; set; }

        public Affiliate AffiliateInfo { get; set; }

        public string ExternalLoginUrl { get; set; }
    }
}
