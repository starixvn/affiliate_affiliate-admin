// --------------------------------------------------------------------------
// <copyright file="SystemSettingDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SystemSettingDetail Criteria class.
    /// </summary>
    public class SystemSettingDetailCriteria : CriteriaBase
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        [SpParam(Ignored = true)]
        public SearchByType SearchBy { get; set; }

        /// <summary>
        /// Get store procedure name by key.
        /// </summary>
        /// <returns>Store procedure name.</returns>
        public override string GetSettingKey()
        {
            switch (SearchBy)
            {
                case SearchByType.ById:
                    return "AffAdmin_SystemSettingsSelByID";
                case SearchByType.ByType:
                    return "AffAdmin_SystemSettingsSelByType";
                default:
                    return "AffAdmin_SystemSettingsSelByName";
            }
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(this.Name) || !string.IsNullOrEmpty(this.Type) || ID > 0;
        }
    }
}