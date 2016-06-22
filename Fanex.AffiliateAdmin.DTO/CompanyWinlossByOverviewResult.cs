namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Company win loss.
    /// </summary>
    public class CompanyWinlossByOverviewResult
    {
        public IEnumerable<CompanyWinLossItem> CompanyWinlossList { get; set; }

        public IEnumerable<CompanyWinLossItem> TotalCompanyWinlossInMemberCurrencies { get; set; }

        public CompanyWinLossItem TotalCompanyWinlossInSystemCurrency { get; set; }
    }
}