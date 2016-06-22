namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Company Win Loss Report By Product.
    /// </summary>
    public class CompanyWinlossReportByProduct
    {
        public string GroupInfo { get; set; }

        public string Level { get; set; }

        public int MergeRowNumber
        {
            get
            {
                return CompanyWinlossInMemberCurrencies.Sum(t => t.MergeRowNumber);
            }
        }

        public IEnumerable<CompanyWinlossByCurrency> CompanyWinlossInMemberCurrencies { get; set; }

        public CompanyWinlossByCurrency CompanyWinlossInSystemCurrency { get; set; }
    }
}