// --------------------------------------------------------------------------
// <copyright file="IReportDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.3.26">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IReportDataAccess
    {
        MemberWinlossReport GetMemberWinloss(MemberWinlossReportCriteria criteria);

        CompanyWinlossReport GetCompanyWinloss(CompanyWinlossReportCriteria criteria);

        CommissionReport GetCommission(AffiliateWinLossCriteria criteria);
    }
}
