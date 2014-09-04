// --------------------------------------------------------------------------
// <copyright file="TierSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.4">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Commission group detail criteria class.
    /// </summary>
    public class TierSettingCriteria : CriteriaBase
    {
        public string GroupIDList { get; set; }

        public DateTime? Date { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupList_SettingDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}