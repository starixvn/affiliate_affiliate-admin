// --------------------------------------------------------------------------
// <copyright file="IAffiliateWinLossByProductDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.18">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAffiliateWinLossByProductDataAccess
    {
        AffiliateWinLossByProductResult GetAffiliateWinLossByProduct(AffiliateWinLossByProductCriteria criteria);
    }
}