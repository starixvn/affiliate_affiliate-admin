// --------------------------------------------------------------------------
// <copyright file="CompanyWinlossReportByOverview.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.11">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyWinlossReportByOverview
    {
        public string GroupInfo { get; set; }

        public string Level { get; set; }

        public IEnumerable<CompanyWinloss> CompanyWinlossInMemberCurrencies { get; set; }

        public CompanyWinloss CompanyWinlossInSystemCurrency { get; set; }
    }
}
