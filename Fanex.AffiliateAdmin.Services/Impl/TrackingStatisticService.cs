// -----------------------------------------------------------------------
// <copyright file="TrackingStatisticService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using Fanex.AffiliateAdmin.Core.Services;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Common.Helper;
    using System.Collections.Generic;

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

            var result = GetDataOutput<ClickStatistic>(criteria);

            foreach (var item in result)
            {
                item.OriginalSportCode = item.SportCode;

                var value = Lang.ResourceManager.GetString(item.SportCode.Replace(" ", string.Empty));

                item.SportCode = string.IsNullOrEmpty(value) ? item.SportCode : value;
            }

            return result;
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