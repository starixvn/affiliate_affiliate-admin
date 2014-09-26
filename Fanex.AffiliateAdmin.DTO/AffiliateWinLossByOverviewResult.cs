// --------------------------------------------------------------------------
// <copyright file="AffiliateWinLossByOverviewResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.17">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateWinLossByOverviewResult
    {
        public IEnumerable<AffiliateWinLossItem> CommissionList { get; set; }

        public IEnumerable<AffiliateWinLossItem> CommissionInAffiliateCurrency { get; set; }

        public AffiliateWinLossItem CommissionInSystemCurrency { get; set; }
    }
}
