// --------------------------------------------------------------------------
// <copyright file="PaymentDetail.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.07.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PaymentDetail
    {
        public long Transid { get; set; }

        public string AffCurrency { get; set; }

        public int AffId { get; set; }

        public DateTime Transdate { get; set; }

        public DateTime CompletedDate { get; set; }

        public long RefId { get; set; }

        public string Status { get; set; }

        public string Transdesc { get; set; }

        public string UserName2 { get; set; }

        public string AffUserName { get; set; }

        public string PreferredPayment { get; set; }

        public string PreferredLanguage { get; set; }

        public decimal Amt { get; set; }

        public decimal AffPayout { get; set; }

        public string AdminDesc { get; set; }

        public string AffDesc { get; set; }

        public decimal TransferAmount { get; set; }

        public string IBCUserName { get; set; }

        public string Email { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Adminname { get; set; }

        public short PlayerCurrency { get; set; }

        public string SystemName { get; set; }

        public short Currency { get; set; }

        public string BalanceUserName { get; set; }

        public string PlayerCurrencyCode { get; set; }
    }
}
