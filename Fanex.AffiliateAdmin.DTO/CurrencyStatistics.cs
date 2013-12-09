// --------------------------------------------------------------------------
// <copyright file="CurrencyStatistics.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// Currency Statistics.
    /// </summary>
    public class CurrencyStatistics
    {
        public string CurrencyCode { get; set; }

        public short? Currency { get; set; }

        public decimal? Total { get; set; }

        public decimal? TotalMembers { get; set; }
    }
}