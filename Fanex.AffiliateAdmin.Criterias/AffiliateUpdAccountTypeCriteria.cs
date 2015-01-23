// --------------------------------------------------------------------------
// <copyright file="AffiliateUpdAccountTypeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.7">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Affiliate Update Account Type Criteria.
    /// </summary>
    public class AffiliateUpdAccountTypeCriteria : CriteriaBase
    {
        public string AffIds { get; set; }

        [SpParam(Ignored = true)]
        public string Usernames { get; set; }

        public byte TestLevel { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateUpdAccountType";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(AffIds);
        }
    }
}