// --------------------------------------------------------------------------
// <copyright file="AffiliateSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.7">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class AffiliateSettings
    {
        private static IAffiliateSettingService _affiliateSettingService = new AffiliateSettingService();

        public static IEnumerable<AccountType> AccountTypes
        {
            get
            {
                var accountTypes = _affiliateSettingService.GetAccountTypeList(new AccountTypeCriteria
                {
                    IsIT = SessionManager.CurrentSession.IsIT
                });

                var translated = accountTypes.Select(t => new AccountType()
                    {
                        TypeID = t.TypeID,
                        TypeName = Lang.ResourceManager.GetString(t.TypeName.Replace(" ", string.Empty).Replace("-", string.Empty))
                    });

                return translated;
            }
        }
    }
}