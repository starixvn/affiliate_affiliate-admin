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
        public IEnumerable<AffiliateWinLoss> CommissionList { get; set; }

        public IEnumerable<AffiliateWinLoss> CommissionInAffiliateCurrency { get; set; }

        public IEnumerable<AffiliateWinLoss> CommissionInSystemCurrency { get; set; }
    }
}
