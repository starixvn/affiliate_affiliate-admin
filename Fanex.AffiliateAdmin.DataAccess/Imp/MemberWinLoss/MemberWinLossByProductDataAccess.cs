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
    public class MemberWinLossByProductDataAccess : MemberWinLossDataAccess, IMemberWinLossByProductDataAccess
    {
        /// <summary>
        /// Get data for Member win-loss.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Member win-loss report.</returns>
        public MemberWinLossByProductResult GetMemberWinLossByProduct(MemberWinlossByProductCriteria criteria)
        {
            var memberWinlossReport = new MemberWinLossByProductResult();

            if (criteria.IsValid())
            {
                using (var objectDb = new SqlObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objectDb.ExecuteReader(criteria))
                    {
                        memberWinlossReport.MemberWinlossList = GetMemberWinlossList(reader);

                        memberWinlossReport.TotalMemberWinlossInMemberCurrencies = GetTotalMemberWinlossInMemberCurrency(reader);

                        memberWinlossReport.TotalMemberWinlossInSystemCurrency = GetTotalMemberWinlossInSystemCurrency(reader);
                    }

                    GetOutputParams(criteria, objectDb);
                }
            }

            return memberWinlossReport;
        }

        protected override void GetColumnExtra(IDataReader reader, MemberWinLossItem memberWinLoss)
        {
            memberWinLoss.ProductId = reader.ToInt("ProductID");

            var productName = reader.ToString("ProductName");
            var translatedProduct = Lang.ResourceManager.GetString(productName.Replace(" ", string.Empty));
            memberWinLoss.ProductName = string.IsNullOrEmpty(translatedProduct) ? productName : translatedProduct;
            memberWinLoss.OriginalProductName = productName;
        }

        protected override void GetColumnExtraInTotalResult(IDataReader reader, MemberWinLossItem memberWinLoss)
        {
            GetColumnExtra(reader, memberWinLoss);
        }
    }
}