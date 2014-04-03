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
    public class MemberWinlossReport
    {
        public IEnumerable<MemberWinloss> MemberWinlossList { get; set; }

        public IEnumerable<MemberWinloss> TotalMemberWinlossInMemberCurrencies { get; set; }

        public MemberWinloss TotalMemberWinlossInSystemCurrency { get; set; }

        public long Duration { get; set; }
    }
}
