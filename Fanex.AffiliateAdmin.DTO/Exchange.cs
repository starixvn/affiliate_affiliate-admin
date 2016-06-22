namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// Exchange class.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    [Serializable]
    public class Exchange
    {                
        public short ExchangeID { get; set; }

        public string Code { get; set; }

        public string Currency { get; set; }

        public int Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsAutoRefresh { get; set; }

        public string Prefix { get; set; }

        public decimal DepositPercent { get; set; }

        public decimal WithdrawalPercent { get; set; }

        public decimal USD { get; set; }

        public decimal USD_Market { get; set; }

        public decimal EUR { get; set; }

        public decimal EUR_Market { get; set; }

        public decimal CNY { get; set; }

        public decimal CNY_Market { get; set; }

        public decimal UUS { get; set; }

        public decimal UUS_Market { get; set; }

        public decimal MYR { get; set; }

        public decimal MYR_Market { get; set; }

        public decimal SGD { get; set; }

        public decimal SGD_Market { get; set; }

        public decimal IDR { get; set; }

        public decimal IDR_Market { get; set; }

        public decimal AUD { get; set; }

        public decimal AUD_Market { get; set; }

        public decimal CHF { get; set; }

        public decimal CHF_Market { get; set; }

        public decimal GBP { get; set; }

        public decimal GBP_Market { get; set; }

        public decimal THB { get; set; }

        public decimal THB_Market { get; set; }

        public decimal VND { get; set; }

        public decimal VND_Market { get; set; }
    }
}