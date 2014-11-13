// --------------------------------------------------------------------------
// <copyright file="IAffiliateDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.1">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.DataAccess
{
    using Criterias;
    using DTO;
    using Models;

    /// <summary>
    /// Interface AffiliateDataAccess.
    /// </summary>
    public interface IBetListDataAccess
    {
        decimal GetExchangeRate(GetExchangeRateCriteria criteria);
    }
}