// --------------------------------------------------------------------------
// <copyright file="IAffiliateSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.7">Created</change>
// </history>

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