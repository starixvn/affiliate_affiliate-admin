// --------------------------------------------------------------------------
// <copyright file="IBetListDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.14">Created</change>
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

        CustomerInfoBetList GetPlayerIbcInfo(string playerUsername);
    }
}