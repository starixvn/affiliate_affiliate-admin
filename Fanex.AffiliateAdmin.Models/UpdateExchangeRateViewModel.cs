namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using Fanex.Data;

    /// <summary>
    /// Update Exchange Rate View Model.
    /// </summary>
    public class UpdateExchangeRateViewModel
    {
        public int UserId { get; set; }

        public int ExchangeId { get; set; }

        public string ExchangeIdList { get; set; }

        public DateTime? Time { get; set; }

        public double Rate { get; set; }

        [SpParam(Ignored = true)]
        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public string Ip { get; set; }
    }
}