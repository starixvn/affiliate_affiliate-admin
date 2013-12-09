// --------------------------------------------------------------------------
// <copyright file="ISystemSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.25">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
    using Fanex.AffiliateAdmin.DTO;

    public interface ISystemSettingRepository
    {
        /// <summary>
        /// Gets system setting.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return object.</returns>
        SystemSetting GetSystemSetting(SystemSettingDetailCriteria criteria);

        /// <summary>
        /// Get currencies of admin.
        /// </summary>
        /// <param name="criteria">Account criteria.</param>
        /// <returns>Currency list.</returns>
        IEnumerable<Currency> GetCurrencies(AccountCriteria criteria);

        /// <summary>
        /// Get system supported currencies.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Currency list.</returns>
        IEnumerable<Currency> GetAffSystemSupportedCurrencies(AccountCriteria criteria);

        /// <summary>
        /// Get system supported currencies (without currency permission).
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Currency list.</returns>
        IEnumerable<Currency> GetAffSystemAllSupportedCurrencies(AccountCriteria criteria);

        /// <summary>
        /// Get system supported Exchange.
        /// </summary>
        /// <param name="criteria">System Setting Exchange Criteria.</param>
        /// <returns>Exchange Rate.</returns>
        Exchange GetSystemBaseExchange(SystemSettingExchangeCriteria criteria);
    }
}