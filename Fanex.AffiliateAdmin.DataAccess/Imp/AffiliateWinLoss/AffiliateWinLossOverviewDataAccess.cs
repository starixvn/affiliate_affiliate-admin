namespace Fanex.AffiliateAdmin.DataAccess
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Data.SqlServer;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Affiliate Win Loss Overview Data Access.
    /// </summary>
    public class AffiliateWinLossOverviewDataAccess : AffiliateWinLossDataAccess, IAffiliateWinLossOverviewDataAccess
    {
        /// <summary>
        /// Get Win Loss Overview.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Win Loss Overview Result.</returns>
        public AffiliateWinLossByOverviewResult GetAffiliateWinLossOverview(AffiliateWinLossOverviewCriteria criteria)
        {
            var commissionReport = new AffiliateWinLossByOverviewResult();

            if (criteria.IsValid())
            {
                using (var objectDb = new SqlObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objectDb.ExecuteReader(criteria))
                    {
                        commissionReport.CommissionList = GetCommissionList(reader);

                        commissionReport.CommissionInAffiliateCurrency = GetTotalCommissionInAffiliateCurrency(reader);

                        commissionReport.CommissionInSystemCurrency = GetTotalCommissionInSystemCurrency(reader).FirstOrDefault();
                    }

                    GetOutputParams(criteria, objectDb);
                }
            }

            return commissionReport;
        }

        protected override void GetColumnExtra(IDataReader reader, AffiliateWinLossItem affiliateWinLoss, string colSuffix)
        {
            SetPropertyValue(reader, affiliateWinLoss, string.Format("Deposit{0}", colSuffix));
            SetPropertyValue(reader, affiliateWinLoss, string.Format("Withdrawal{0}", colSuffix));
            SetPropertyValue(reader, affiliateWinLoss, string.Format("PSP{0}", colSuffix));
            SetPropertyValue(reader, affiliateWinLoss, string.Format("Bonus{0}", colSuffix));
            SetPropertyValue(reader, affiliateWinLoss, string.Format("Adjustment{0}", colSuffix));
        }
    }
}