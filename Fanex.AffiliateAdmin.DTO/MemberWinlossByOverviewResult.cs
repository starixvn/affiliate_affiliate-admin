// --------------------------------------------------------------------------
// <copyright file="MemberWinlossByOverviewResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Member Win Loss By Overview Result.
    /// </summary>
    public class MemberWinLossByOverviewResult
    {
        public IEnumerable<MemberWinLossItem> MemberWinlossList { get; set; }

        public IEnumerable<MemberWinLossItem> TotalMemberWinlossInMemberCurrencies { get; set; }

        public MemberWinLossItem TotalMemberWinlossInSystemCurrency { get; set; }

        public double Duration { get; set; }
    }
}