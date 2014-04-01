// --------------------------------------------------------------------------
// <copyright file="CompanyWinlossReport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.4.1">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Company win loss.
    /// </summary>
    public class CompanyWinlossReport
    {
        public IEnumerable<CompanyWinloss> CompanyWinlossList { get; set; }

        public IEnumerable<CompanyWinloss> CompanyWinlossInMemberCurrencies { get; set; }

        public CompanyWinloss CompanyWinlossInSystemCurrency { get; set; }
    }
}