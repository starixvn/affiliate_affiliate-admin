// --------------------------------------------------------------------------
// <copyright file="SystemSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.25">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Repositories.Impl
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Repositories.Interface;
    using Fanex.Common.Helper;
    using Fanex.Data;

    /// <summary>
    /// SystemSetting Repository class.
    /// </summary>
    public class SystemSettingRepository : ISystemSettingRepository
    {
        /// <summary>
        /// Get system setting.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>System setting object.</returns>
        public SystemSetting GetSystemSetting(SystemSettingDetailCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.QueryEntity<SystemSetting>(criteria.GetSpParams());
                }
            }

            return new SystemSetting();
        }

        /// <summary>
        /// Get currencies of admin.
        /// </summary>
        /// <param name="criteria">Account criteria.</param>
        /// <returns>Currency list.</returns>
        public IEnumerable<Currency> GetCurrencies(AccountCriteria criteria)
        {
            criteria.Type = "UserCurrency";
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var currentParams = criteria.GetSpParams();
                return objectDb.Query<Currency>(currentParams);
            }
        }

        public IEnumerable<Currency> GetAffSystemSupportedCurrencies(AccountCriteria criteria)
        {
            criteria.Type = "SystemCurrency";
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var currentParams = criteria.GetSpParams();
                return objectDb.Query<Currency>(currentParams);
            }
        }

        public IEnumerable<Currency> GetAffSystemAllSupportedCurrencies(AccountCriteria criteria)
        {
            criteria.Type = "AllSystemCurrency";
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var currentParams = criteria.GetSpParams();
                return objectDb.Query<Currency>(currentParams);
            }
        }

        public Exchange GetSystemBaseExchange(SystemSettingExchangeCriteria criteria)
        {
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDb.QueryEntity<Exchange>();
            }
        }
    }
}