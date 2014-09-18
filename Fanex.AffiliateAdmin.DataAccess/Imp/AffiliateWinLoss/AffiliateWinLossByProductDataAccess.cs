// --------------------------------------------------------------------------
// <copyright file="AffiliateWinLossByProductDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.18">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Data;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Common.Extensions;
    using Fanex.Data;

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
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    var reader = objectDb.ExecuteReader(param);

                    commissionReport.CommissionList = GetCommissionList(reader);

                    commissionReport.CommissionInAffiliateCurrency = GetTotalCommissionInAffiliateCurrency(reader);

                    commissionReport.CommissionInSystemCurrency = GetTotalCommissionInSystemCurrency(reader);

                    reader.Close();

                    GetOutputParams(criteria, objectDb);
                }
            }

            return commissionReport;
        }

        protected override void GetColumnExtra(IDataReader reader, AffiliateWinLoss affiliateWinLoss)
        {
            affiliateWinLoss.ProductId = reader.ToInt("ProductId");
            affiliateWinLoss.ProductName = reader.ToString("ProductName");
        }
    }
}