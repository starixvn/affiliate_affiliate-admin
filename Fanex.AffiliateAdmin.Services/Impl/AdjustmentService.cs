// -----------------------------------------------------------------------
// <copyright file="AdjustmentService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using Fanex.AffiliateAdmin.Core.Services;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Common.Helper;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AdjustmentService : QueryService, IAdjustmentService
    {
        /// <summary>
        /// Get Adjustment Balance.
        /// </summary>
        /// <param name="criteria">Adjustment criteria.</param>
        /// <returns>Adjustment information.</returns>
        public Adjustment GetAdjustmentBalance(AdjustmentCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetData<Adjustment>(criteria).FirstOrDefault();
        }

        /// <summary>
        /// Update Adjustment.
        /// </summary>
        /// <param name="criteria">Adjustment information criteria.</param>
        public void UpdateAdjustment(AdjustmentEditCriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}
