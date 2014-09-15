// --------------------------------------------------------------------------
// <copyright file="ReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.DataAccess;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class ReportService : QueryService, IReportService
    {
        private readonly IReportDataAccess _reportDataAccess;

        private readonly ICompanyWinLossByProductDataAccess _companyWinLossByProductDataAccess;

        private readonly ICompanyWinLossOverviewDataAccess _companyWinLossOverviewDataAccess;

        public ReportService()
        {
            _reportDataAccess = new ReportDataAccess();

            _companyWinLossByProductDataAccess = new CompanyWinLossByProductDataAccess();

            _companyWinLossOverviewDataAccess = new CompanyWinLossOverviewDataAccess();
        }

        public CompanyWinlossByProductResult GetCompanyWinlossByProduct(CompanyWinlossReportCriteria criteria)
        {
            return _companyWinLossByProductDataAccess.GetCompanyWinLossByProduct(criteria);
        }

        public CompanyWinlossOverviewResult GetCompanyWinlossOverview(CompanyWinlossReportCriteria criteria)
        {
            return _companyWinLossOverviewDataAccess.GetCompanyWinLossOverview(criteria);
        }

        public IEnumerable<MemberStatistic> GetMemberStatistics(MemberStatisticReportCriteria criteria)
        {
            return GetData<MemberStatistic>(criteria);
        }
        
        public MemberWinLossReport GetMemberWinloss(MemberWinlossReportCriteria criteria)
        {
            return _reportDataAccess.GetMemberWinLoss(criteria);
        }

        public IEnumerable<QualifiedMember> GetQualifiedMembers(QualifiedMembersReportCriteria criteria)
        {
            return GetData<QualifiedMember>(criteria);
        }

        AffiliateWinLossReport IReportService.GetAffiliateWinLoss(AffiliateWinLossCriteria criteria)
        {
            return _reportDataAccess.GetAffiliateWinLoss(criteria);
        }

        public IEnumerable<AffiliateCommission> GetAffiliateCommission(AffiliateCommissionCriteria criteria)
        {
            return GetDataOutput<AffiliateCommission>(criteria);
        }
    }
}