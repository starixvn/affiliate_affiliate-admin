// --------------------------------------------------------------------------
// <copyright file="AffiliateCommissionByProductDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.22">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Data;

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
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    var reader = objectDb.ExecuteReader(param);

                    commissionReport.AffiliateCommissionList = GetAffiliateCommissionList(reader);

                    commissionReport.AffiliateCommissionInAffiliateCurrency = GetTotalAffiliateCommissionInAffiliateCurrency(reader);

                    commissionReport.AffiliateCommissionInSystemCurrency = GetTotalAffiliateCommissionInSystemCurrency(reader);

                    reader.Close();

                    GetOutputParams(criteria, objectDb);
                }
            }

            return commissionReport;
        }
    }
}