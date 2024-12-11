namespace Fanex.AffiliateAdmin.Services
{
    using Criterias;
    using DTO;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPermissionCurrencyService
    {
        IEnumerable<PermissionCurrency> GetPermissionCurrencyList(UserCurrencySelCriteria criteria);

        void UpdatePermissionCurrency(UserCurrencyInsCriteria criteria);
    }
}
