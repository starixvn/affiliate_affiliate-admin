// --------------------------------------------------------------------------
// <copyright file="IReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services.Interface
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IReportService
    {
        CompanyWinlossReport GetCompanyWinloss(CompanyWinlossReportCriteria criteria);

        IEnumerable<MemberStatistic> GetMemberStatistics(MemberStatisticReportCriteria criteria);

        IEnumerable<CommissionReport> GetCommission(CommissionReportCriteria criteria);

        MemberWinlossReport GetMemberWinloss(PlayerWinlossReportCriteria criteria);

        IEnumerable<QualifiedMember> GetQualifiedMembers(QualifiedMembersReportCriteria criteria);
    }
}