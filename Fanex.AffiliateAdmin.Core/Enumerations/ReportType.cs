// --------------------------------------------------------------------------
// <copyright file="ReportType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.5">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Enumerations
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// No report.
        /// </summary>
        None,

        /// <summary>
        /// Company Win Loss report.
        /// </summary>
        CompanyWinLoss,

        /// <summary>
        /// Member Win Loss report.
        /// </summary>
        MemberWinLoss,

        /// <summary>
        /// Affiliate Win Loss report.
        /// </summary>
        AffiliateWinLoss,

        /// <summary>
        /// Affiliate Commission report.
        /// </summary>
        AffiliateCommission
    }
}
