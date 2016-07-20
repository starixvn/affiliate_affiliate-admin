namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateWinLossByOverviewResult
    {
        public IEnumerable<AffiliateWinLossItem> CommissionList { get; set; }

        public IEnumerable<AffiliateWinLossItem> CommissionInAffiliateCurrency { get; set; }

        public AffiliateWinLossItem CommissionInSystemCurrency { get; set; }
    }
}
