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
    using DataAccess.Imp;
    using DataAccess.Interface;
    using DTO;
    using Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportService : QueryService, IReportService
    {
        public IEnumerable<CompanyWinloss> GetCompanyWinloss(CompanyWinlossReportCriteria criteria)
        {
            return GetData<CompanyWinloss>(criteria);
        }
        
        public IEnumerable<MemberStatistic> GetMemberStatistics(MemberStatisticReportCriteria criteria)
        {
            return GetData<MemberStatistic>(criteria);
        }

        public IEnumerable<CommissionReport> GetCommission(CommissionReportCriteria criteria)
        {
            return GetData<CommissionReport>(criteria);
        }
        
        public IEnumerable<PlayerWinloss> GetPlayerWinloss(PlayerWinlossReportCriteria criteria)
        {
            return GetData<PlayerWinloss>(criteria);
        }

        public IEnumerable<QualifiedMember> GetQualifiedMembers(QualifiedMembersReportCriteria criteria)
        {
            return GetData<QualifiedMember>(criteria);
        }
    }
}