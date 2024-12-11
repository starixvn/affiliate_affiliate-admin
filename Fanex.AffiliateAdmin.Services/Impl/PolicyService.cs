// -----------------------------------------------------------------------
// <copyright file="PolicyService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

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
        public IEnumerable<FunctionPolicy> GetGroupPolicyList(GroupPolicySelCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetData<FunctionPolicy>(criteria);
        }

        /// <summary>
        /// Get list policy of user.
        /// </summary>
        /// <param name="criteria">Policy criteria.</param>
        /// <returns>Function policy list.</returns>
        public IEnumerable<FunctionPolicy> GetUserPolicyList(UserPolicySelCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = GetData<FunctionPolicy>(criteria);

            foreach (var policy in result)
            {
                var translated = Lang.ResourceManager.GetString(policy.FuncDesc.Replace(" ", string.Empty));
                policy.FuncDesc = string.IsNullOrEmpty(translated) ? policy.FuncDesc : translated;
            }

            return result;
        }

        /// <summary>
        /// Update policy of user.
        /// </summary>
        /// <param name="criteria">Policy update criteria.</param>
        public void UpdatePolicy(ICriteria criteria)
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