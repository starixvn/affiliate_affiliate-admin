// --------------------------------------------------------------------------
// <copyright file="CommissionReport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommissionReport
    {
        public IEnumerable<CommissionItem> CommissionList { get; set; }

        public IEnumerable<CommissionItem> CommissionInAffiliateCurrency { get; set; }

        public CommissionItem CommissionInSystemCurrency { get; set; }
    }
}
