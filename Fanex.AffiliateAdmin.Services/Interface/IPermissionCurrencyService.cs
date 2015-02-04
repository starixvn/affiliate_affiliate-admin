// -----------------------------------------------------------------------
// <copyright file="IPermissionCurrencyService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// --------------------------------------------------------------------------
// <history>
//   <change who="Muller.Nguyen" date="2013.09.01">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPermissionCurrencyService
    {
        IEnumerable<PermissionCurrency> GetPermissionCurrencyList(UserCurrencySelCriteria criteria);

        void UpdatePermissionCurrency(UserCurrencyInsCriteria criteria);
    }
}
