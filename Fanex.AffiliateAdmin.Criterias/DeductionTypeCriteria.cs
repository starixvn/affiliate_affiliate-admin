// --------------------------------------------------------------------------
// <copyright file="DeductionTypeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Deduction Type Criteria.
    /// </summary>
    public class DeductionTypeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_DeductionTypeSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}