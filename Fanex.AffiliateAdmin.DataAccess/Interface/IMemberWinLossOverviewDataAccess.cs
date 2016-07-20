namespace Fanex.AffiliateAdmin.DataAccess
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMemberWinLossOverviewDataAccess
    {
        MemberWinLossByOverviewResult GetMemberWinLossOverview(MemberWinLossOverviewCriteria criteria);
    }
}