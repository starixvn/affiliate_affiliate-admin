namespace Fanex.AffiliateAdmin.DataAccess
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Data.SqlServer;

    /// <summary>
    /// Affiliate Commission By Product Data Access.
    /// </summary>
    public class AffiliateCommissionByProductDataAccess : AffiliateCommissionDataAccess, IAffiliateCommissionDataAccess
    {
        /// <summary>
        /// Get Win Loss Overview.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Win Loss Overview Result.</returns>
        public AffiliateCommissionResult GetAffiliateCommissionByProduct(AffiliateCommissionCriteria criteria)
        {
            var commissionReport = new AffiliateCommissionResult();

            if (criteria.IsValid())
            {
                using (var objectDb = new SqlObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objectDb.ExecuteReader(criteria))
                    {
                        commissionReport.AffiliateCommissionList = GetAffiliateCommissionList(reader);

                        commissionReport.AffiliateCommissionInAffiliateCurrency = GetTotalAffiliateCommissionInAffiliateCurrency(reader);

                        commissionReport.AffiliateCommissionInSystemCurrency = GetTotalAffiliateCommissionInSystemCurrency(reader);
                    }

                    GetOutputParams(criteria, objectDb);
                }
            }

            return commissionReport;
        }
    }
}