// -----------------------------------------------------------------------
// <copyright file="AdjustmentCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AdjustmentCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public string Currency { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_CustomerSelBalBF";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
