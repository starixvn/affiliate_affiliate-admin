// -----------------------------------------------------------------------
// <copyright file="AffiliateInfoViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models.AffiliateInfo
{
    using System;
    using System.Collections.Generic;
    using DTO;
    using Core.Configurations;
    using Core.Session;

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