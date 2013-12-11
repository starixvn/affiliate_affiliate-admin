// --------------------------------------------------------------------------
// <copyright file="ProcessingComm.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.12.10">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ProcessingComm
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

        public decimal? CommissionBase { get; set; }

        public decimal? CashbackPercent { get; set; }

        public decimal? CashbackAmount { get; set; }

        public decimal? CashbackAmountBase { get; set; }

        public decimal? SportsbookWinloss { get; set; }

        public int? SportsbookQM { get; set; }

        public decimal? SportsbookCommissionPercent { get; set; }

        public decimal? SportsbookCommission { get; set; }

        public decimal? SportsbookWinlossBase { get; set; }

        public decimal? SportsbookCommissionBase { get; set; }

        public decimal? NumberGameWinloss { get; set; }

        public int? NumberGameQM { get; set; }

        public decimal? NumberGameCommissionPercent { get; set; }

        public decimal? NumberGameCommission { get; set; }

        public decimal? NumberGameWinlossBase { get; set; }

        public decimal? NumberGameCommissionBase { get; set; }

        public decimal? LiveCasinoWinloss { get; set; }

        public int? LiveCasinoQM { get; set; }

        public decimal? LiveCasinoCommissionPercent { get; set; }

        public decimal? LiveCasinoCommission { get; set; }

        public decimal? LiveCasinoWinlossBase { get; set; }

        public decimal? LiveCasinoCommissionBase { get; set; }

        public decimal? RNGCasinoWinloss { get; set; }

        public int? RNGCasinoQM { get; set; }

        public decimal? RNGCasinoCommissionPercent { get; set; }

        public decimal? RNGCasinoCommission { get; set; }

        public decimal? RNGCasinoWinlossBase { get; set; }

        public decimal? RNGCasinoCommissionBase { get; set; }

        public long? RowNo { get; set; }

        public long? RowNum { get; set; }
    }
}