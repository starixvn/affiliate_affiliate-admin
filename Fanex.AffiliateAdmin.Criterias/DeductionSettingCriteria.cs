// --------------------------------------------------------------------------
// <copyright file="DeductionSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.10">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// DeductionSetting Criteria class.
    /// </summary>
    public class DeductionSettingCriteria : CriteriaBase
    {
        public string GroupIDList { get; set; }

        public DateTime? Date { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DeductionSettingSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}