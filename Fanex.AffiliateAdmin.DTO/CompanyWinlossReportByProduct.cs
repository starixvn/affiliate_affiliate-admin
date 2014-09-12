// --------------------------------------------------------------------------
// <copyright file="CompanyWinlossReportByProduct.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.11">Created</change>
// </history>

using System.Linq;

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Company Win Loss Report By Product.
    /// </summary>
    public class CompanyWinlossReportByProduct
    {
        public string GroupInfo { get; set; }

        public string Level { get; set; }

        public int MergeRowNumber 
        {
            get
            {
                return  CompanyWinlossInMemberCurrencies.Sum(t => t.MergeRowNumber);
            }
        }

        public IEnumerable<CompanyWinlossByCurrency> CompanyWinlossInMemberCurrencies { get; set; }

        public CompanyWinloss CompanyWinlossInSystemCurrency { get; set; }
    }
}