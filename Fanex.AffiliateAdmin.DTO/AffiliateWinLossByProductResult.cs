namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateWinLossByProductResult
    {
        public IEnumerable<AffiliateWinLossItem> AffiliateWinLossList { get; set; }

        public IEnumerable<AffiliateWinLossItem> AffiliateWinLossInAffiliateCurrency { get; set; }

        public IEnumerable<AffiliateWinLossItem> AffiliateWinLossInSystemCurrency { get; set; }
    }
}
