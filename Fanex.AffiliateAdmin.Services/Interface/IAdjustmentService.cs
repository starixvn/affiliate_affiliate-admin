// -----------------------------------------------------------------------
// <copyright file="IAdjustmentService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAdjustmentService
    {
        /// <summary>
        /// Get Adjustment Balance.
        /// </summary>
        /// <param name="criteria">Adjustment criteria.</param>
        /// <returns>Adjustment information.</returns>
        Adjustment GetAdjustmentBalance(AdjustmentCriteria criteria);

        /// <summary>
        /// Update Adjustment.
        /// </summary>
        /// <param name="criteria">Adjustment information criteria.</param>
        void UpdateAdjustment(AdjustmentEditCriteria criteria);
    }
}
