// -----------------------------------------------------------------------
// <copyright file="ClickStatisticDetailCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ClickStatisticDetailCriteria : BaseCriteria
    {
        public int AffId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Domain { get; set; }

        public string SportCode { get; set; }

        public string LanguageCode { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_ClickStatisticsDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
