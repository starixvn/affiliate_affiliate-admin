// -----------------------------------------------------------------------
// <copyright file="PermissionCurrencyService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PermissionCurrencyService : QueryService, IPermissionCurrencyService
    {
        /// <summary>
        /// Get list permission currency of user.
        /// </summary>
        /// <param name="criteria">Permission currency criteria.</param>
        /// <returns>Permission currency list.</returns>
        public IEnumerable<PermissionCurrency> GetPermissionCurrencyList(PermissionCurrencyCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetData<PermissionCurrency>(criteria);
        }

        /// <summary>
        /// Update permission currency of user.
        /// </summary>
        /// <param name="criteria">Permission currency update criteria.</param>
        public void UpdatePermissionCurrency(PermissionCurrencyCriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}
