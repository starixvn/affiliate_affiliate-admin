namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    public class CompanyWinlossReportByProduct
    {
        public string GroupInfo { get; set; }

        public string Level { get; set; }

        public IEnumerable<CompanyWinlossByCurrency> CompanyWinlossInMemberCurrencies { get; set; }

        public CompanyWinloss CompanyWinlossInSystemCurrency { get; set; }
    }
}