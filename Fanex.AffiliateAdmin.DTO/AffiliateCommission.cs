// --------------------------------------------------------------------------
// <copyright file="AffiliateCommission.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.4">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateCommission
    {
        public int? AffId { get; set; }

        public string Status { get; set; }

        public DateTime? WinlostDate { get; set; }

        public string Username { get; set; }

        public int? ExchangeId { get; set; }

        public string Currency { get; set; }

        public int? GroupID { get; set; }

        public string GroupName { get; set; }

        public decimal? Commission { get; set; }

        public double? CommissionBase { get; set; }

        public decimal? SportsbookWinloss { get; set; }

        public int? SportsbookQM { get; set; }

        public decimal? SportsbookCommissionPercent { get; set; }

        public decimal? SportsbookCommission { get; set; }

        public double? SportsbookWinlossBase { get; set; }

        public double? SportsbookCommissionBase { get; set; }

        public decimal? NumberGameWinloss { get; set; }

        public int? NumberGameQM { get; set; }

        public decimal? NumberGameCommissionPercent { get; set; }

        public decimal? NumberGameCommission { get; set; }

        public double? NumberGameWinlossBase { get; set; }

        public double? NumberGameCommissionBase { get; set; }

        public decimal? LiveCasinoWinloss { get; set; }

        public int? LiveCasinoQM { get; set; }

        public decimal? LiveCasinoCommissionPercent { get; set; }

        public decimal? LiveCasinoCommission { get; set; }

        public double? LiveCasinoWinlossBase { get; set; }

        public double? LiveCasinoCommissionBase { get; set; }

        public decimal? RNGCasinoWinloss { get; set; }

        public int? RNGCasinoQM { get; set; }

        public decimal? RNGCasinoCommissionPercent { get; set; }

        public decimal? RNGCasinoCommission { get; set; }

        public double? RNGCasinoWinlossBase { get; set; }

        public double? RNGCasinoCommissionBase { get; set; }

        public decimal? PlaytechCasinoWinloss { get; set; }

        public int? PlaytechCasinoQM { get; set; }

        public decimal? PlaytechCasinoCommissionPercent { get; set; }

        public decimal? PlaytechCasinoCommission { get; set; }

        public double? PlaytechCasinoWinlossBase { get; set; }

        public double? PlaytechCasinoCommissionBase { get; set; }

        public decimal? VirtualSportWinloss { get; set; }

        public int? VirtualSportQM { get; set; }

        public decimal? VirtualSportCommissionPercent { get; set; }

        public decimal? VirtualSportCommission { get; set; }

        public double? VirtualSportWinlossBase { get; set; }

        public double? VirtualSportCommissionBase { get; set; }

        public long? RowNo { get; set; }

        public decimal PShare { get; set; }

        public decimal PWin { get; set; }

        public decimal NetGaming { get; set; }

        public double PShareBase { get; set; }

        public double PWinBase { get; set; }

        public double NetGamingBase { get; set; }
    }
}