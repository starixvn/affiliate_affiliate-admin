// --------------------------------------------------------------------------
// <copyright file="CommGroupSelActiveListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.8.29">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommGroupSelActiveListCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string Currency { get; set; }

        public int LicenseeId { get; set; } 
        
        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroup_SelActiveList";
        }

        public override bool IsValid()
        {
            return UserId > 0 && !string.IsNullOrEmpty(Currency);
        }
    }
}
