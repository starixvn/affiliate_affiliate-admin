namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Affiliate Commission Result.
    /// </summary>
    public class AffiliateCommissionResult
    {
        public IEnumerable<AffiliateCommission> AffiliateCommissionList { get; set; }

        public IEnumerable<AffiliateCommission> AffiliateCommissionInAffiliateCurrency { get; set; }

        public IEnumerable<AffiliateCommission> AffiliateCommissionInSystemCurrency { get; set; }
    }
}