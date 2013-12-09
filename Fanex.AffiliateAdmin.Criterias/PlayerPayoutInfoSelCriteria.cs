// --------------------------------------------------------------------------
// <copyright file="PlayerPayoutInfoSelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.7.31">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerPayoutInfoSelCriteria : CriteriaBase
    {
        public long AffId { get; set; }

        public long RefId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_PlayerPayoutInfoSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}