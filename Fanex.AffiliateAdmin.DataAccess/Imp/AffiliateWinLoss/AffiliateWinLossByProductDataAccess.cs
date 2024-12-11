namespace Fanex.AffiliateAdmin.DataAccess
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Common.Extensions;
    using Fanex.Data.SqlServer;
    using System.Data;

    /// <summary>
    /// Affiliate Win Loss Overview Data Access.
    /// </summary>
    public class AffiliateWinLossByProductDataAccess : AffiliateWinLossDataAccess, IAffiliateWinLossByProductDataAccess
    {
        /// <summary>
        /// Get Win Loss Overview.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Win Loss Overview Result.</returns>
        public AffiliateWinLossByProductResult GetAffiliateWinLossByProduct(AffiliateWinLossByProductCriteria criteria)
        {
            var commissionReport = new AffiliateWinLossByProductResult();

            if (criteria.IsValid())
            {
                using (var objectDb = new SqlObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objectDb.ExecuteReader(criteria))
                    {
                        commissionReport.AffiliateWinLossList = GetCommissionList(reader);

                        commissionReport.AffiliateWinLossInAffiliateCurrency = GetTotalCommissionInAffiliateCurrency(reader);

                        commissionReport.AffiliateWinLossInSystemCurrency = GetTotalCommissionInSystemCurrency(reader);
                    }

                    GetOutputParams(criteria, objectDb);
                }
            }

            return commissionReport;
        }

        protected override void GetColumnExtra(IDataReader reader, AffiliateWinLossItem affiliateWinLoss)
        {
            affiliateWinLoss.ProductId = reader.ToInt("ProductId");
            var productName = reader.ToString("ProductName");
            var translatedProduct = Lang.ResourceManager.GetString(productName.Replace(" ", string.Empty));
            affiliateWinLoss.ProductName = string.IsNullOrEmpty(translatedProduct) ? productName : translatedProduct;
        }
    }
}