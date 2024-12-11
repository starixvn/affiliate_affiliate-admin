namespace Fanex.AffiliateAdmin.DataAccess
{
    using Common.Extensions;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Data.SqlServer;
    using System.Data;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class CompanyWinLossByProductDataAccess : CompanyWinLossDataAccess, ICompanyWinLossByProductDataAccess
    {
        /// <summary>
        /// Get data for company win-loss.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Company win-loss report.</returns>
        public CompanyWinlossByProductResult GetCompanyWinLossByProduct(CompanyWinlossByProductCriteria criteria)
        {
            var companyWinlossReport = new CompanyWinlossByProductResult();

            if (criteria.IsValid())
            {
                using (var objectDb = new SqlObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objectDb.ExecuteReader(criteria))
                    {
                        companyWinlossReport.CompanyWinlossList = GetCompanyWinLossByProductList(reader, criteria.BaseCurrency, criteria.GroupBy);

                        companyWinlossReport.TotalCompanyWinlossInMemberCurrencies = GetCompanyWinLossSumUpByMemberCurrency(reader, criteria.BaseCurrency);

                        companyWinlossReport.TotalCompanyWinlossInSystemCurrency = GetCompanyWinLossSumUpBySystemCurrency(reader);
                    }

                    GetOutputParams(criteria, objectDb);
                }
            }

            return companyWinlossReport;
        }

        protected override void GetColumnExtra(IDataReader reader, CompanyWinLossItem companyWinloss)
        {
            companyWinloss.ProductID = reader.ToInt("ProductID");

            var productName = reader.ToString("ProductName");
            var translatedProduct = Lang.ResourceManager.GetString(productName.Replace(" ", string.Empty));
            companyWinloss.ProductName = string.IsNullOrEmpty(translatedProduct) ? productName : translatedProduct;
        }
    }
}