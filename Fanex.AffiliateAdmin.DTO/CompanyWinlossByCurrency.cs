// --------------------------------------------------------------------------
// <copyright file="CompanyWinlossByCurrency.cs" company="Nexcel Solutions Vietnam">
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
    public class CompanyWinlossByCurrency
    {
        public string Currency { get; set; }

        public IEnumerable<CompanyWinloss> CompanyWinlossByProductList { get; set; }

        public CompanyWinloss TotalCompanyWinlossByProduct { get; set; }
    }
}
