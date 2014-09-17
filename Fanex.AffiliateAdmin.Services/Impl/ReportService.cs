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

        private readonly IAffiliateWinLossOverviewDataAccess _affiliateWinLossOverviewDataAccess;

        public ReportService()
        {
            _reportDataAccess = new ReportDataAccess();

            _companyWinLossByProductDataAccess = new CompanyWinLossByProductDataAccess();

            _companyWinLossOverviewDataAccess = new CompanyWinLossOverviewDataAccess();

            _affiliateWinLossOverviewDataAccess = new AffiliateWinLossOverviewDataAccess();
        }

        public CompanyWinlossByProductResult GetCompanyWinlossByProduct(CompanyWinlossByProductCriteria criteria)
        {
            return _companyWinLossByProductDataAccess.GetCompanyWinLossByProduct(criteria);
        }

        public CompanyWinlossByOverviewResult GetCompanyWinlossOverview(CompanyWinlossOverviewCriteria criteria)
        {
            return _companyWinLossOverviewDataAccess.GetCompanyWinLossOverview(criteria);
        }

        public IEnumerable<MemberStatistic> GetMemberStatistics(MemberStatisticReportCriteria criteria)
        {
            return GetData<MemberStatistic>(criteria);
        }

        public AffiliateWinLossByOverviewResult GetAffiliateWinLoss(AffiliateWinLossOverviewCriteria criteria)
        {
            return _affiliateWinLossOverviewDataAccess.GetAffiliateWinLossOverview(criteria);
        }

        public MemberWinLossReport GetMemberWinloss(MemberWinlossReportCriteria criteria)
        {
            return _reportDataAccess.GetMemberWinLoss(criteria);
        }

        public IEnumerable<QualifiedMember> GetQualifiedMembers(QualifiedMembersReportCriteria criteria)
        {
            return GetData<QualifiedMember>(criteria);
        }

        public IEnumerable<AffiliateCommission> GetAffiliateCommission(AffiliateCommissionCriteria criteria)
        {
            return GetDataOutput<AffiliateCommission>(criteria);
        }
    }
}