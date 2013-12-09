// --------------------------------------------------------------------------
// <copyright file="CompanyWinloss.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// Company win loss.
    /// </summary>
    public class CompanyWinloss
    {
        public DateTime WinlostDate { get; set; }

        public string Currency { get; set; }

        public short? CurrencyId { get; set; }

        public double? ExRate { get; set; }

        public decimal? Deposit { get; set; }

        public decimal? Withdrawal { get; set; }
        
        public decimal? Bonus { get; set; }

        public decimal? Adjustment { get; set; }

        public decimal? PSP { get; set; }

        public decimal? SportsBookTurnover { get; set; }

        public decimal? NumberGameTurnover { get; set; }

        public decimal? LiveCasinoTurnover { get; set; }

        public decimal? RNGCasinoTurnover { get; set; }

        public decimal? Turnover { get; set; }

        public int? BetCount { get; set; }

        public int? ActivePlayer { get; set; }

        public decimal? Winlost { get; set; }

        public decimal? SportsBookWinlost { get; set; }

        public decimal? NumberGameWinlost { get; set; }

        public decimal? LiveCasinoWinlost { get; set; }

        public decimal? RNGCasinoWinlost { get; set; }

        public decimal? Total { get; set; }

        public double? DepositBase { get; set; }

        public double? WithdrawalBase { get; set; }

        public double? BonusBase { get; set; }

        public double? AdjustmentBase { get; set; }

        public double? PSPBase { get; set; }

        public double? SportsBookTurnoverBase { get; set; }

        public double? NumberGameTurnoverBase { get; set; }

        public double? LiveCasinoTurnoverBase { get; set; }

        public double? RNGCasinoTurnoverBase { get; set; }

        public double TurnoverBase { get; set; }

        public double? WinlostBase { get; set; }

        public double? SportsBookWinlostBase { get; set; }

        public double? NumberGameWinlostBase { get; set; }

        public double? LiveCasinoWinlostBase { get; set; }

        public double? RNGCasinoWinlostBase { get; set; }

        public double? TotalBase { get; set; }
    }
}