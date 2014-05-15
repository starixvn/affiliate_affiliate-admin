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

        public decimal TotalTurnover { get; set; }

        public int BetCount { get; set; }

        public int ActivePlayer { get; set; }

        public decimal TotalMemberWinlost { get; set; }

        public decimal SportsbookWinlost { get; set; }

        public decimal NumberGameWinlost { get; set; }

        public decimal LiveCasinoWinlost { get; set; }

        public decimal RNGCasinoWinlost { get; set; }

        public decimal PlaytechCasinoWinlost { get; set; }

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

        public decimal TotalTurnoverBase { get; set; }

        public decimal TotalMemberWinlostBase { get; set; }

        public decimal SportsbookWinlostBase { get; set; }

        public decimal NumberGameWinlostBase { get; set; }

        public decimal LiveCasinoWinlostBase { get; set; }

        public decimal RNGCasinoWinlostBase { get; set; }

        public decimal PlaytechCasinoWinlostBase { get; set; }

        public decimal TotalCompanyWinlostBase 
        { 
            get 
            { 
                return -TotalMemberWinlostBase; 
            } 
        }

        public decimal Deposit_Aff { get; set; }

        public decimal Withdrawal_Aff { get; set; }

        public decimal Bonus_Aff { get; set; }

        public decimal Adjustment_Aff { get; set; }

        public decimal PSP_Aff { get; set; }

        public decimal TotalTurnover_Aff { get; set; }

        public decimal SportsbookTurnover_Aff { get; set; }

        public decimal NumberGameTurnover_Aff { get; set; }

        public decimal LiveCasinoTurnover_Aff { get; set; }

        public decimal RNGCasinoTurnover_Aff { get; set; }

        public decimal PlaytechCasinoTurnover_Aff { get; set; }

        public decimal TotalMemberWinlost_Aff { get; set; }

        public decimal SportsbookWinlost_Aff { get; set; }

        public decimal NumberGameWinlost_Aff { get; set; }

        public decimal LiveCasinoWinlost_Aff { get; set; }

        public decimal RNGCasinoWinlost_Aff { get; set; }

        public decimal PlaytechCasinoWinlost_Aff { get; set; }

        public decimal TotalCompanyWinlost_Aff 
        { 
            get 
            { 
                return -TotalMemberWinlost_Aff; 
            } 
        }
    }
}