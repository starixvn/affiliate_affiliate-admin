// --------------------------------------------------------------------------
// <copyright file="CompanyWinlossByOverviewResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Company win loss.
    /// </summary>
    public class CompanyWinlossByOverviewResult
    {
        public IEnumerable<CompanyWinloss> CompanyWinlossList { get; set; }

        public IEnumerable<CompanyWinloss> TotalCompanyWinlossInMemberCurrencies { get; set; }

        public CompanyWinloss TotalCompanyWinlossInSystemCurrency { get; set; }
    }
}