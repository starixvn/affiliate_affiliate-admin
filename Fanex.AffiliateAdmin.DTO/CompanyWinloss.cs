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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Company win loss.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public class CompanyWinloss
    {
        public DateTime WinlostDate { get; set; }

        public string Currency { get; set; }

        public short CurrencyId { get; set; }

        public double ExRate { get; set; }

        public double AffExRate { get; set; }

        public decimal Deposit { get; set; }

        public decimal Withdrawal { get; set; }

        public decimal Bonus { get; set; }

        public decimal Adjustment { get; set; }

        public decimal PSP { get; set; }

        public decimal SportsbookTurnover { get; set; }

        public decimal NumberGameTurnover { get; set; }

        public decimal LiveCasinoTurnover { get; set; }

        public decimal RNGCasinoTurnover { get; set; }

        public decimal PlaytechCasinoTurnover { get; set; }

        public decimal VirtualSportTurnover { get; set; }

        public decimal TotalTurnover { get; set; }

        public int BetCount { get; set; }

        public int ActivePlayer { get; set; }

        public decimal TotalMemberWinlost { get; set; }

        public decimal SportsbookWinlost { get; set; }

        public decimal NumberGameWinlost { get; set; }

        public decimal LiveCasinoWinlost { get; set; }

        public decimal RNGCasinoWinlost { get; set; }

        public decimal PlaytechCasinoWinlost { get; set; }

        public decimal VirtualSportWinlost { get; set; }

        public decimal TotalCompanyWinlost 
        { 
            get 
            { 
                return -TotalMemberWinlost; 
            } 
        }

        public decimal DepositBase { get; set; }

        public decimal WithdrawalBase { get; set; }

        public decimal BonusBase { get; set; }

        public decimal AdjustmentBase { get; set; }

        public decimal PSPBase { get; set; }

        public decimal SportsbookTurnoverBase { get; set; }

        public decimal NumberGameTurnoverBase { get; set; }

        public decimal LiveCasinoTurnoverBase { get; set; }

        public decimal RNGCasinoTurnoverBase { get; set; }

        public decimal PlaytechCasinoTurnoverBase { get; set; }

        public decimal VirtualSportTurnoverBase { get; set; }

        public decimal TotalTurnoverBase { get; set; }

        public decimal TotalMemberWinlostBase { get; set; }

        public decimal SportsbookWinlostBase { get; set; }

        public decimal NumberGameWinlostBase { get; set; }

        public decimal LiveCasinoWinlostBase { get; set; }

        public decimal RNGCasinoWinlostBase { get; set; }

        public decimal PlaytechCasinoWinlostBase { get; set; }

        public decimal VirtualSportWinlostBase { get; set; }

        public decimal TotalCompanyWinlostBase 
        { 
            get 
            { 
                return -TotalMemberWinlostBase; 
            } 
        }

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
        public decimal SportsbookTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal NumberGameTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal LiveCasinoTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal RNGCasinoTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal PlaytechCasinoTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal VirtualSportTurnover_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal TotalMemberWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal SportsbookWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal NumberGameWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal LiveCasinoWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal RNGCasinoWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal PlaytechCasinoWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal VirtualSportWinlost_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal TotalCompanyWinlost_Aff 
        { 
            get 
            { 
                return -TotalMemberWinlost_Aff; 
            } 
        }

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

        public decimal SportsbookPlayerComm { get; set; }

        public decimal NumberGamePlayerComm { get; set; }

        public decimal LiveCasinoPlayerComm { get; set; }

        public decimal RNGCasinoPlayerComm { get; set; }

        public decimal PlaytechCasinoPlayerComm { get; set; }

        public decimal VirtualSportPlayerComm { get; set; }

        public decimal SportsbookPlayerCommBase { get; set; }

        public decimal NumberGamePlayerCommBase { get; set; }

        public decimal LiveCasinoPlayerCommBase { get; set; }

        public decimal RNGCasinoPlayerCommBase { get; set; }

        public decimal PlaytechCasinoPlayerCommBase { get; set; }

        public decimal VirtualSportPlayerCommBase { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal SportsbookPlayerComm_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal NumberGamePlayerComm_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal LiveCasinoPlayerComm_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal RNGCasinoPlayerComm_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal PlaytechCasinoPlayerComm_Aff { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public decimal VirtualSportPlayerComm_Aff { get; set; }
    }
}