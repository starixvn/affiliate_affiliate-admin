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
        IEnumerable<Commission> CommissionList { get; set; }

        IEnumerable<Commission> CommissionInSystemCurrency { get; set; }
    }
}
