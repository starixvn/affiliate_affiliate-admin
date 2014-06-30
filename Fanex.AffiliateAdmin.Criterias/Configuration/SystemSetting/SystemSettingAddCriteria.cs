// --------------------------------------------------------------------------
// <copyright file="SystemSettingAddCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// SystemSettingAdd Criteria class.
    /// </summary>
    public class SystemSettingAddCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public string Type { get; set; }

        public string Description { get; set; }

        public string Environment { get; set; }

        public string IP { get; set; }

        public int ID { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSettingsIns";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Key) && !string.IsNullOrEmpty(Value);
        }
    }
}