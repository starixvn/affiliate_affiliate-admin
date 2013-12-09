// --------------------------------------------------------------------------
// <copyright file="SystemSettingSelectCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SystemSettingSelect Criteria class.
    /// </summary>
    public class SystemSettingSelectCriteria : CriteriaBase
    {
        public string Type { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSettingsSelAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}