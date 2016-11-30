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
        #region Fields

        private IList<AffiliateOnUser> _onlineUsers = new List<AffiliateOnUser>();

        #endregion Fields

        public AffiliateStatisticViewModel StatisticViewModel { get; set; }

        public IList<AffiliateOnUser> OnUsers
        {
            get
            {
                return _onlineUsers;
            }
        }

        public Affiliate AffiliateInfo { get; set; }

        public Uri ExternalLoginUrl { get; set; }

        public bool HasHostAffiliatePolicy
        {
            get
            {
                return SessionManager.CurrentSession.HasHostPolicy(FunctionPolicyNames.Affiliate);
            }
        }
        
        public void AddOnlineUsers(IList<AffiliateOnUser> onlineUsers)
        {
            _onlineUsers = onlineUsers;
        }
    }
}