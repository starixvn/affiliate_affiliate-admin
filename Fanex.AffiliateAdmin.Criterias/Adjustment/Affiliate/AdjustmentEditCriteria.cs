// -----------------------------------------------------------------------
// <copyright file="AdjustmentEditCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AdjustmentEditCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public int AdminId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string AffDescription { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public string AdminUserName { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_AdjustmentIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
