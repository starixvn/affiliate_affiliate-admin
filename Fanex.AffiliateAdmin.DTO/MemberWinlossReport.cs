// --------------------------------------------------------------------------
// <copyright file="PlayerWinlossReport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.3.26">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberWinlossReport
    {
        public MemberWinlossReport()
        {
            PlayerWinlossList = new List<MemberWinloss>();
            PlayerWinlossInMemberCurrencies = new List<MemberWinloss>();
        }

        public IEnumerable<MemberWinloss> PlayerWinlossList { get; set; }

        public IEnumerable<MemberWinloss> PlayerWinlossInMemberCurrencies { get; set; }

        public MemberWinloss PlayerWinlossInSystemCurrency { get; set; }

        public long Duration { get; set; }
    }
}
