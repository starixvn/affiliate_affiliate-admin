// --------------------------------------------------------------------------
// <copyright file="SystemSettingExchangeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.09.26">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SystemSettingUpdate Criteria class.
    /// </summary>
    public class SystemSettingExchangeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSettingsExchange";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}