// --------------------------------------------------------------------------
// <copyright file="CompanyWinLossOverviewDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Data;
    using Criterias;
    using DTO;
    using Fanex.Data;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class CompanyWinLossOverviewDataAccess : CompanyWinLossDataAccess, ICompanyWinLossOverviewDataAccess
    {
        /// <summary>
        /// Get Company Win Loss Overview.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Company Win Loss By Overview Result.</returns>
        public CompanyWinlossByOverviewResult GetCompanyWinLossOverview(CompanyWinlossOverviewCriteria criteria)
        {
            var companyWinlossReport = new CompanyWinlossByOverviewResult();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    using (var reader = objectDb.ExecuteReader(param))
                    {
                        companyWinlossReport.CompanyWinlossList = GetCompanyWinLossByProductList(reader, criteria.BaseCurrency, criteria.GroupBy);

                        companyWinlossReport.TotalCompanyWinlossInMemberCurrencies = GetCompanyWinLossSumUpByMemberCurrency(reader, criteria.BaseCurrency);
                    }

                    GetOutputParams(criteria, objectDb);
                }
            }

            return companyWinlossReport;
        }

        /// <summary>
        /// Get Column Extra.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="companyWinloss">Company Win Loss.</param>
        /// <param name="colSuffix">Column Suffix.</param>
        protected override void GetColumnExtra(IDataReader reader, CompanyWinLossItem companyWinloss, string colSuffix)
        {
            SetPropertyValue(reader, companyWinloss, string.Format("Deposit{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("Withdrawal{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("Bonus{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("Adjustment{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("PSP{0}", colSuffix));
        }
    }
}