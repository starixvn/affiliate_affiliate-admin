// --------------------------------------------------------------------------
// <copyright file="SystemSettingDeleteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SystemSettingDelete Criteria class.
    /// </summary>
    public class SystemSettingDeleteCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public string IDList { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSettingsDel";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(IDList);
        }
    }
}