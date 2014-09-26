// --------------------------------------------------------------------------
// <copyright file="AffiliateWinLossByProductResult.cs" company="Nexcel Solutions Vietnam">
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
    public class AffiliateWinLossByProductResult
    {
        public IEnumerable<AffiliateWinLossItem> AffiliateWinLossList { get; set; }

        public IEnumerable<AffiliateWinLossItem> AffiliateWinLossInAffiliateCurrency { get; set; }

        public IEnumerable<AffiliateWinLossItem> AffiliateWinLossInSystemCurrency { get; set; }
    }
}
