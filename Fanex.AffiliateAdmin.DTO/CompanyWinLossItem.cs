// --------------------------------------------------------------------------
// <copyright file="CompanyWinLossItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Company win loss.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public class CompanyWinLossItem
    {
        public long RowIdx { get; set; }

        public DateTime WinlostDate { get; set; }

        public string Currency { get; set; }

        public short CurrencyId { get; set; }

        public double ExRate { get; set; }

        public double AffExRate { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public int BetCount { get; set; }

        public int ActivePlayer { get; set; }

        public decimal Deposit { get; set; }

        public decimal Withdrawal { get; set; }

        public decimal Bonus { get; set; }

        public decimal Adjustment { get; set; }

        public decimal PSP { get; set; }

        public decimal TotalTurnover { get; set; }

        public decimal TotalTurnoverBase { get; set; }

        public decimal CompanyWinLoss { get; set; }
       
        public decimal DepositBase { get; set; }

        public decimal WithdrawalBase { get; set; }

        public decimal BonusBase { get; set; }

        public decimal AdjustmentBase { get; set; }

        public decimal PSPBase { get; set; }

        public decimal CompanyWinLossBase { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal Deposit_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal Withdrawal_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal Bonus_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal Adjustment_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal PSP_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal TotalTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal CompanyWinLoss_Aff { get; set; }

        public decimal PShare { get; set; }

        public decimal PWin { get; set; }

        public decimal NetGaming { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal PShare_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal PWin_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal NetGaming_Aff { get; set; }

        public decimal PShareBase { get; set; }

        public decimal PWinBase { get; set; }

        public decimal NetGamingBase { get; set; }

        public decimal PlayerComm { get; set; }

        public decimal PlayerCommBase { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal PlayerComm_Aff { get; set; }
    }
}