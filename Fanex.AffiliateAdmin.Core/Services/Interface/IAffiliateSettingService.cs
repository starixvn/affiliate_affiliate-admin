namespace Fanex.AffiliateAdmin.Core.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// Affiliate Setting Service.
    /// </summary>
    public interface IAffiliateSettingService
    {
        /// <summary>
        /// Get Account Type List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Account Type List.</returns>
        IEnumerable<AccountType> GetAccountTypeList(AccountTypeCriteria criteria);
    }
}