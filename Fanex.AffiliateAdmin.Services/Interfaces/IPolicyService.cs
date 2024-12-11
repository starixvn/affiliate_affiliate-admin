// -----------------------------------------------------------------------
// <copyright file="IPolicyService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using Criterias;
    using DTO;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

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
        IEnumerable<FunctionPolicy> GetGroupPolicyList(GroupPolicySelCriteria criteria);

        /// <summary>
        /// Get list policy of user.
        /// </summary>
        /// <param name="criteria">Policy criteria.</param>
        /// <returns>Function policy list.</returns>
        IEnumerable<FunctionPolicy> GetUserPolicyList(UserPolicySelCriteria criteria);

        /// <summary>
        /// Update policy of user.
        /// </summary>
        /// <param name="criteria">Policy update criteria.</param>
        void UpdatePolicy(ICriteria criteria);

        /// <summary>
        /// Get list policy of user.
        /// </summary>
        /// <param name="criteria">Policy criteria.</param>
        /// <returns>Function policy list.</returns>
        IEnumerable<FunctionPolicy> GetPolicyMultiUserList(PolicyMultiUserCriteria criteria);
    }
}