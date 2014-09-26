// --------------------------------------------------------------------------
// <copyright file="MemberWinlossByProductResult.cs" company="Nexcel Solutions Vietnam">
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
    /// Member Win Loss By Product Result.
    /// </summary>
    public class MemberWinLossByProductResult
    {
        public IEnumerable<MemberWinLossItem> MemberWinlossList { get; set; }

        public IEnumerable<MemberWinLossItem> TotalMemberWinlossInMemberCurrencies { get; set; }

        public IEnumerable<MemberWinLossItem> TotalMemberWinlossInSystemCurrency { get; set; }
    }
}