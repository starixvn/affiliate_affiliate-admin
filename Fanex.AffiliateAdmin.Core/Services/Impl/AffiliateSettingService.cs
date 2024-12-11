namespace Fanex.AffiliateAdmin.Core.Services.Impl
{
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateSettingService : QueryService, IAffiliateSettingService
    {
        /// <summary>
        /// Get Account Type List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Account Type List.</returns>
        public IEnumerable<AccountType> GetAccountTypeList(AccountTypeCriteria criteria)
        {
            return GetData<AccountType>(criteria);
        }
    }
}