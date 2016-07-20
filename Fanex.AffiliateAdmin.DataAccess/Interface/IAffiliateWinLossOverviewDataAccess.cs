namespace Fanex.AffiliateAdmin.DataAccess
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAffiliateWinLossOverviewDataAccess
    {
        AffiliateWinLossByOverviewResult GetAffiliateWinLossOverview(AffiliateWinLossOverviewCriteria criteria);
    }
}