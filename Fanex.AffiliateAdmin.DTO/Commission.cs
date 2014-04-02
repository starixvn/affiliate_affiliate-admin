// --------------------------------------------------------------------------
// <copyright file="CommissionReport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Commission
    {
        public int AffId { get; set; }

        public string Username { get; set; }

        public string AffType { get; set; }

        public string Status { get; set; }

        public string Language { get; set; }

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public DateTime? WinlostDate { get; set; }

        public string Currency { get; set; }

        public int ActivePlayers { get; set; }

        public int BetCount { get; set; }

        public decimal Commission { get; set; }

        public decimal Deposit { get; set; }

        public decimal Withdrawal { get; set; }

        public decimal Turnover { get; set; }

        public decimal Winloss { get; set; }

        public double CommissionBase { get; set; }

        public decimal DepositBase { get; set; }

        public decimal WithdrawalBase { get; set; }

        public decimal TurnoverBase { get; set; }

        public decimal WinlossBase { get; set; }

        public int SportsbookActivePlayers { get; set; }

        public int SportsbookBetCount { get; set; }

        public decimal SportsbookTurnover { get; set; }

        public decimal SportsbookWinloss { get; set; }

        public decimal SportsbookCommission { get; set; }

        public decimal SportsbookTurnoverBase { get; set; }

        public decimal SportsbookWinlossBase { get; set; }

        public decimal SportsbookCommissionBase { get; set; }

        public int SportsbookQM { get; set; }

        public decimal SportsbookCommissionPercent { get; set; }

        public int NumberGameActivePlayers { get; set; }

        public int NumberGameBetCount { get; set; }

        public decimal NumberGameTurnover { get; set; }

        public decimal NumberGameWinloss { get; set; }

        public decimal NumberGameCommission { get; set; }

        public decimal NumberGameTurnoverBase { get; set; }

        public decimal NumberGameWinlossBase { get; set; }

        public decimal NumberGameCommissionBase { get; set; }

        public int NumberGameQM { get; set; }

        public decimal NumberGameCommissionPercent { get; set; }

        public int LiveCasinoActivePlayers { get; set; }

        public int LiveCasinoBetCount { get; set; }

        public decimal LiveCasinoTurnover { get; set; }

        public decimal LiveCasinoWinloss { get; set; }

        public decimal LiveCasinoCommission { get; set; }

        public decimal LiveCasinoTurnoverBase { get; set; }

        public decimal LiveCasinoWinlossBase { get; set; }

        public decimal LiveCasinoCommissionBase { get; set; }

        public int LiveCasinoQM { get; set; }

        public decimal LiveCasinoCommissionPercent { get; set; }

        public int RNGCasinoActivePlayers { get; set; }

        public int RNGCasinoBetCount { get; set; }

        public decimal RNGCasinoTurnover { get; set; }

        public decimal RNGCasinoWinloss { get; set; }

        public decimal RNGCasinoCommission { get; set; }

        public decimal RNGCasinoTurnoverBase { get; set; }

        public decimal RNGCasinoWinlossBase { get; set; }

        public decimal RNGCasinoCommissionBase { get; set; }

        public int RNGCasinoQM { get; set; }

        public decimal RNGCasinoCommissionPercent { get; set; }

        public decimal DeductedPercentage { get; set; }

        public decimal Cashback { get; set; }

        public double CashbackBase { get; set; }
    }
}
