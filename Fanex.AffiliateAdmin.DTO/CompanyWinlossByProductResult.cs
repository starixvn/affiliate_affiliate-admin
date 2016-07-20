namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Company win loss.
    /// </summary>
    public class CompanyWinlossByProductResult
    {
        public IEnumerable<CompanyWinLossItem> CompanyWinlossList { get; set; }

        public IEnumerable<CompanyWinLossItem> TotalCompanyWinlossInMemberCurrencies { get; set; }

        public IEnumerable<CompanyWinLossItem> TotalCompanyWinlossInSystemCurrency { get; set; }
    }
}