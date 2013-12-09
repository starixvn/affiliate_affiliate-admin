// -----------------------------------------------------------------------
// <copyright file="PolicyService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services.Impl
{
    using System.Collections.Generic;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PolicyService : QueryService, IPolicyService
    {
        /// <summary>
        /// Get list policy of user.
        /// </summary>
        /// <param name="criteria">Policy criteria.</param>
        /// <returns>Function policy list.</returns>
        public IEnumerable<FunctionPolicy> GetPolicyList(PolicyCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetData<FunctionPolicy>(criteria);
        }

        /// <summary>
        /// Update policy of user.
        /// </summary>
        /// <param name="criteria">Policy update criteria.</param>
        public void UpdatePolicy(PolicyUpdateCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Get list policy of user.
        /// </summary>
        /// <param name="criteria">Policy criteria.</param>
        /// <returns>Function policy list.</returns>
        public IEnumerable<FunctionPolicy> GetPolicyMultiUserList(PolicyMultiUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetData<FunctionPolicy>(criteria);
        }
    }
}
