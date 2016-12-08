namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IReportService
    {
        CompanyWinlossByProductResult GetCompanyWinlossByProduct(CompanyWinlossByProductCriteria criteria);

        CompanyWinlossByOverviewResult GetCompanyWinlossOverview(CompanyWinlossOverviewCriteria criteria);

        IEnumerable<MemberStatistic> GetMemberStatistics(MemberStatisticReportCriteria criteria);

        AffiliateWinLossByOverviewResult GetAffiliateWinLossOverview(AffiliateWinLossOverviewCriteria criteria);

        AffiliateWinLossByProductResult GetAffiliateWinLossByProduct(AffiliateWinLossByProductCriteria criteria);

        MemberWinLossByProductResult GetMemberWinlossByProduct(MemberWinlossByProductCriteria criteria);

        MemberWinLossByOverviewResult GetMemberWinlossOverview(MemberWinLossOverviewCriteria criteria);

        IEnumerable<QualifiedMember> GetQualifiedMembers(QualifiedMembersReportCriteria criteria);

        AffiliateCommissionResult GetAffiliateCommission(AffiliateCommissionCriteria criteria);
    }
}