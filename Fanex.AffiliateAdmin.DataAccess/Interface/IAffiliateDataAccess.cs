namespace Fanex.AffiliateAdmin.DataAccess
{
    using Criterias;
    using DTO;
    using Models;

    /// <summary>
    /// Interface AffiliateDataAccess.
    /// </summary>
    public interface IAffiliateDataAccess
    {
        AffiliateInfoViewModel GetAffiliateGeneralInfo(AffiliateInfoCriteria criteria);

        AffiliateStatisticViewModel GetAffiliateStatistic(AffiliateStatisticCriteria criteria);

        Affiliate GetAffiliateInfo(AffiliateSelCriteria criteria);

        AffiliateListReportViewModel GetAffiliateList(AffiliateListCriteria criteria);
    }
}