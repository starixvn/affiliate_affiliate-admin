// --------------------------------------------------------------------------
// <copyright file="ExchangeRate.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.29">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// Exchange Rate class.
    /// </summary>
    public class ExchangeRate
    {
        public short ExchangeID { get; set; }

        public string Code { get; set; }

        public string Currency { get; set; }

        public int Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public int MultipliedBy { get; set; }

        public bool IsAutoRefresh { get; set; }

        public string Prefix { get; set; }

        public double DepositPercent { get; set; }

        public double WithdrawalPercent { get; set; }

        public double USD { get; set; }

        public double USD_Market { get; set; }

        public double EUR { get; set; }

        public double EUR_Market { get; set; }

        public double CNY { get; set; }

        public double CNY_Market { get; set; }

        public double UUS { get; set; }

        public double UUS_Market { get; set; }

        public double MYR { get; set; }

        public double MYR_Market { get; set; }

        public double SGD { get; set; }

        public double SGD_Market { get; set; }

        public double IDR { get; set; }

        public double IDR_Market { get; set; }

        public double AUD { get; set; }

        public double AUD_Market { get; set; }

        public double CHF { get; set; }

        public double CHF_Market { get; set; }

        public double GBP { get; set; }

        public double GBP_Market { get; set; }

        public double THB { get; set; }

        public double THB_Market { get; set; }

        public double VND { get; set; }

        public double VND_Market { get; set; }

        public double JPY { get; set; }

        public double JPY_Market { get; set; }

        public double KRW { get; set; }

        public double KRW_Market { get; set; }
    }
}