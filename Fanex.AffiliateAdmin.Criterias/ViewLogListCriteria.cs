// -----------------------------------------------------------------------
// <copyright file="ViewLogListCriteria.cs" company="Microsoft">
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
    public class ViewLogListCriteria : BaseCriteria
    {
        public int AffId { get; set; }

        public int LogType { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int TotalPage { get; set; }

        public int RowCount { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateSelLog";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
