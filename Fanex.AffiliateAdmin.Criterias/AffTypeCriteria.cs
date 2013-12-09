// --------------------------------------------------------------------------
// <copyright file="AffTypeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffTypeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_AffTypeSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}