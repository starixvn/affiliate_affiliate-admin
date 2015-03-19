// --------------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.14">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Criteria;
    using Models;

    /// <summary>
    /// Interface Affiliate service.
    /// </summary>
    public interface IBetListService
    {
        decimal GetExchangeRate(GetExchangeRateCriteria criteria);

        /// <summary>
        /// Get player's username.
        /// </summary>
        /// <param name="playerUsername">Affiliate username.</param>
        /// <returns>Return player's Info.</returns>
        CustomerInfoBetList GetPlayerIbcInfo(string playerUsername);

        List<RNGCasinoResult> GetRNGCasinoResult(BetListResultsRNGCasinoCriteria criteria);

        BetListResultsRNGCasinoDetailViewModel GetRNGCasinoResultDetail(BetListResultsRNGCasinoDetailCriteria criteria);

        void ValidatePermission(ValidationCriteria criteria);
    }
}