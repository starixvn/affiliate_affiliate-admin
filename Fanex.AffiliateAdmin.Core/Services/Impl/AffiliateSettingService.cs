// --------------------------------------------------------------------------
// <copyright file="AffiliateSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Services.Impl
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

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