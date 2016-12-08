// -----------------------------------------------------------------------
// <copyright file="ITrackingStatisticService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITrackingStatisticService
    {
        /// <summary>
        /// Get referrers.
        /// </summary>
        /// <param name="criteria">Click statistic by site criteria.</param>
        /// <returns>List click statistic.</returns>
        IEnumerable<ClickStatistic> GetReferrers(ClickStatisticBySiteCriteria criteria);

        /// <summary>
        /// Get Landing pages.
        /// </summary>
        /// <param name="criteria">Click statistic by page criteria.</param>
        /// <returns>List click statistic.</returns>
        IEnumerable<ClickStatistic> GetLandingPages(ClickStatisticByPageCriteria criteria);

        /// <summary>
        /// Get tracking statistic detail.
        /// </summary>
        /// <param name="criteria">Click statistic by site criteria.</param>
        /// <returns>List click statistic.</returns>
        IEnumerable<ClickStatisticDetail> GetTrackingStatisticDetails(ClickStatisticDetailCriteria criteria);
    }
}
