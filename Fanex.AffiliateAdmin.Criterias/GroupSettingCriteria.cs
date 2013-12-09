// --------------------------------------------------------------------------
// <copyright file="GroupSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.24">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GroupSettingCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_GroupSettingSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
