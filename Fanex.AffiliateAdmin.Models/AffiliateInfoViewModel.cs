// -----------------------------------------------------------------------
// <copyright file="AffiliateInfoViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Session;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateInfoViewModel
    {
        public AffiliateStatisticViewModel StatisticViewModel { get; set; }

        public IEnumerable<AffiliateOnUser> OnUsers { get; set; }

        public Affiliate AffiliateInfo { get; set; }

        public Uri ExternalLoginUrl { get; set; }

        public bool HasHostAffiliatePolicy
        {
            get
            {
                return SessionManager.CurrentSession.HasHostPolicy(FunctionPolicyNames.Affiliate);
            }
        }
    }
}