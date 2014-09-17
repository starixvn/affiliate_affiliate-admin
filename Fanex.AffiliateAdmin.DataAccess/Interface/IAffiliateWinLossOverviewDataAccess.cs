// --------------------------------------------------------------------------
// <copyright file="IAffiliateWinLossOverviewDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.17">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAffiliateWinLossOverviewDataAccess
    {
        AffiliateWinLossByOverviewResult GetAffiliateWinLossOverview(AffiliateWinLossOverviewCriteria criteria);
    }
}