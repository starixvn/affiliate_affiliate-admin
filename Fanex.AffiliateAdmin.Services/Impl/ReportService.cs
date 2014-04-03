// --------------------------------------------------------------------------
// <copyright file="ReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services.Impl
{
    using System.Collections.Generic;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.DataAccess.Imp;
    using Fanex.AffiliateAdmin.DataAccess.Interface;
    using Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportService : QueryService, IReportService
    {
        private readonly IReportDataAccess _reportDataAccess;

        public ReportService()
        {
            _reportDataAccess = new ReportDataAccess();
        }

        public CompanyWinlossReport GetCompanyWinloss(CompanyWinlossReportCriteria criteria)
        {
            return _reportDataAccess.GetCompanyWinloss(criteria);
        }
        
        public IEnumerable<MemberStatistic> GetMemberStatistics(MemberStatisticReportCriteria criteria)
        {
            return GetData<MemberStatistic>(criteria);
        }
        
        public MemberWinlossReport GetMemberWinloss(MemberWinlossReportCriteria criteria)
        {
            return _reportDataAccess.GetMemberWinloss(criteria);
        }

        public IEnumerable<QualifiedMember> GetQualifiedMembers(QualifiedMembersReportCriteria criteria)
        {
            return GetData<QualifiedMember>(criteria);
        }

        CommissionReport IReportService.GetCommission(CommissionReportCriteria criteria)
        {
            return _reportDataAccess.GetCommission(criteria);
        }
    }
}