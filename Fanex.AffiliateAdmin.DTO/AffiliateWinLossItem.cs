// --------------------------------------------------------------------------
// <copyright file="AffiliateWinLossItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.5">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public class AffiliateWinLossItem
    {
        public long RowIdx { get; set; }

        public int AffId { get; set; }

        public string Username { get; set; }

        public string AffType { get; set; }

        public string Status { get; set; }

        public string Language { get; set; }

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public DateTime? WinlostDate { get; set; }

        public int CurrencyId { get; set; }

        public string Currency { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int ActivePlayers { get; set; }

        public int BetCount { get; set; }

        public decimal Deposit { get; set; }

        public decimal DepositBase { get; set; }

        public decimal Withdrawal { get; set; }

        public decimal WithdrawalBase { get; set; }

        public decimal Bonus { get; set; }

        public decimal BonusBase { get; set; }

        public decimal Adjustment { get; set; }

        public decimal AdjustmentBase { get; set; }

        public decimal PSP { get; set; }

        public decimal PSPBase { get; set; }

        public decimal PlayerComm { get; set; }

        public decimal PlayerCommBase { get; set; }

        public decimal Turnover { get; set; }

        public decimal TurnoverBase { get; set; }

        public decimal AffiliateWinLoss { get; set; }

        public decimal AffiliateWinLossBase { get; set; }

        public decimal PShare { get; set; }

        public decimal PShareBase { get; set; }

        public decimal PWin { get; set; }

        public decimal PWinBase { get; set; }

        public decimal NetGaming { get; set; }

        public decimal NetGamingBase { get; set; }
    }
}