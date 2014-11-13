// --------------------------------------------------------------------------
// <copyright file="IAffiliateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.1">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;
    using Models;

    /// <summary>
    /// Interface Affiliate service.
    /// </summary>
    public interface IBetListService
    {
        decimal GetExchangeRate(GetExchangeRateCriteria criteria);
    }
}