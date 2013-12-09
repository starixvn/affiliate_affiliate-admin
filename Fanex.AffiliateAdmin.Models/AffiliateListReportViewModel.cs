// --------------------------------------------------------------------------
// <copyright file="AffiliateListReportViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateListReportViewModel
    {
        public AffiliateListReportViewModel()
        {
            AffiliateList = new List<AffiliateInfo>();
            CurrencyList = new List<CurrencyStatistics>();
        }

        public List<AffiliateInfo> AffiliateList { get; set; }

        public List<CurrencyStatistics> CurrencyList { get; set; }
    }
}
