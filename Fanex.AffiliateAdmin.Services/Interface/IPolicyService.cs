// -----------------------------------------------------------------------
// <copyright file="IPolicyService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services.Interface
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPolicyService
    {
        /// <summary>
        /// Get list policy of user.
        /// </summary>
        /// <param name="criteria">Policy criteria.</param>
        /// <returns>Function policy list.</returns>
        IEnumerable<FunctionPolicy> GetPolicyList(PolicyCriteria criteria);

        /// <summary>
        /// Update policy of user.
        /// </summary>
        /// <param name="criteria">Policy update criteria.</param>
        void UpdatePolicy(PolicyUpdateCriteria criteria);

        /// <summary>
        /// Get list policy of user.
        /// </summary>
        /// <param name="criteria">Policy criteria.</param>
        /// <returns>Function policy list.</returns>
        IEnumerable<FunctionPolicy> GetPolicyMultiUserList(PolicyMultiUserCriteria criteria);
    }
}
