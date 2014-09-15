// --------------------------------------------------------------------------
// <copyright file="ReportDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.3.26">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Data;
    using Criterias;
    using DTO;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class CompanyWinLossOverviewDataAccess : CompanyWinLossDataAccess, ICompanyWinLossOverviewDataAccess
    {
        public CompanyWinlossOverviewResult GetCompanyWinLossOverview(CompanyWinlossReportCriteria criteria)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetColumnExtra(IDataReader reader, CompanyWinloss companyWinloss, string suffix)
        {
            SetPropertyValue(reader, companyWinloss, string.Format("Deposit{0}", suffix));
            SetPropertyValue(reader, companyWinloss, string.Format("Withdrawal{0}", suffix));
            SetPropertyValue(reader, companyWinloss, string.Format("Bonus{0}", suffix));
            SetPropertyValue(reader, companyWinloss, string.Format("Adjustment{0}", suffix));
            SetPropertyValue(reader, companyWinloss, string.Format("PSP{0}", suffix));
        }
    }
}