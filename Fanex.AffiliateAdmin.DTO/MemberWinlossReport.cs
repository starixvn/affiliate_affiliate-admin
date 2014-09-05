// --------------------------------------------------------------------------
// <copyright file="MemberWinlossReport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.4.3">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberWinLossReport
    {
        public IEnumerable<MemberWinLoss> MemberWinlossList { get; set; }

        public IEnumerable<MemberWinLoss> TotalMemberWinlossInMemberCurrencies { get; set; }

        public MemberWinLoss TotalMemberWinlossInSystemCurrency { get; set; }

        public long Duration { get; set; }
    }
}
