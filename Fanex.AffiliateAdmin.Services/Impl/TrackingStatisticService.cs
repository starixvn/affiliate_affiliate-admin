// -----------------------------------------------------------------------
// <copyright file="TrackingStatisticService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Core.Services;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Common.Helper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TrackingStatisticService : QueryService, ITrackingStatisticService
    {
        /// <summary>
        /// Get referrers.
        /// </summary>
        /// <param name="criteria">Click statistic by site criteria.</param>
        /// <returns>List click statistic.</returns>
        public IEnumerable<ClickStatistic> GetReferrers(ClickStatisticBySiteCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetData<ClickStatistic>(criteria);
        }

        /// <summary>
        /// Get Landing pages.
        /// </summary>
        /// <param name="criteria">Click statistic by page criteria.</param>
        /// <returns>List click statistic.</returns>
        public IEnumerable<ClickStatistic> GetLandingPages(ClickStatisticByPageCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetDataOutput<ClickStatistic>(criteria);
        }

        /// <summary>
        /// Get tracking statistic detail.
        /// </summary>
        /// <param name="criteria">Click statistic by site criteria.</param>
        /// <returns>List click statistic.</returns>
        public IEnumerable<ClickStatisticDetail> GetTrackingStatisticDetails(ClickStatisticDetailCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetDataOutput<ClickStatisticDetail>(criteria);
        }
    }
}
