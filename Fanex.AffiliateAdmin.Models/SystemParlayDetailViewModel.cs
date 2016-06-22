namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SystemParlayDetailViewModel
    {
        [SpParam(Ignored = true)]
        public string AffiliateGroup { get; set; }

        public int UserId { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public long BetId { get; set; }

        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        // Output parameters
        public decimal Outstanding { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}
