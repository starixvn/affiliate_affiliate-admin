// --------------------------------------------------------------------------
// <copyright file="MemberWinloss.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.11.6">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Player win loss class.
    /// </summary>
    public class MemberWinloss
    {
        public DateTime WinlostDate { get; set; }

        public string Username { get; set; }

        public string Currency { get; set; }

        public string PlayerUsername { get; set; }

        public string PlayerCurrency { get; set; }

        public double ExRate { get; set; }

        public double AffExRate { get; set; }

        public decimal Deposit { get; set; }

        public decimal Withdrawal { get; set; }

        public decimal Bonus { get; set; }

        public decimal PSP { get; set; }

        public decimal Adjustment { get; set; }

        public decimal SportsbookTurnover { get; set; }

        public decimal NumberGameTurnover { get; set; }

        public decimal LiveCasinoTurnover { get; set; }

        public decimal RNGCasinoTurnover { get; set; }

        public decimal PlaytechCasinoTurnover { get; set; }

        public decimal TotalTurnover { get; set; }

        public decimal SportsbookWinlost { get; set; }

        public decimal NumberGameWinlost { get; set; }

        public decimal LiveCasinoWinlost { get; set; }

        public decimal RNGCasinoWinlost { get; set; }

        public decimal PlaytechCasinoWinlost { get; set; }

        public decimal TotalWinlost { get; set; }

        public int BetCount { get; set; }

        public double DepositBase { get; set; }

        public double WithdrawalBase { get; set; }

        public double BonusBase { get; set; }

        public double PSPBase { get; set; }

        public double AdjustmentBase { get; set; }

        public double SportsbookTurnoverBase { get; set; }

        public double NumberGameTurnoverBase { get; set; }

        public double LiveCasinoTurnoverBase { get; set; }

        public double RNGCasinoTurnoverBase { get; set; }

        public double PlaytechCasinoTurnoverBase { get; set; }

        public double TotalTurnoverBase { get; set; }

        public double SportsbookWinlostBase { get; set; }

        public double NumberGameWinlostBase { get; set; }

        public double LiveCasinoWinlostBase { get; set; }

        public double RNGCasinoWinlostBase { get; set; }

        public double PlaytechCasinoWinlostBase { get; set; }

        public double TotalWinlostBase { get; set; }

        public decimal CustomerWinlost
        {
            get { return -TotalWinlost; }
        }

        public double CustomerWinlostBase 
        {
            get { return -TotalWinlostBase;  }
        }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double CustomerWinlost_Aff
        {
            get { return -TotalWinlost_Aff; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double Deposit_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double Withdrawal_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double Bonus_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double Adjustment_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double PSP_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double SportsbookTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double NumberGameTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double LiveCasinoTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double RNGCasinoTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double PlaytechCasinoTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double TotalTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double SportsbookWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double NumberGameWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double LiveCasinoWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double RNGCasinoWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double PlaytechCasinoWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public double TotalWinlost_Aff { get; set; }

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
    }
}