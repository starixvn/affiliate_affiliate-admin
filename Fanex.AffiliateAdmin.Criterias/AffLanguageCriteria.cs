// --------------------------------------------------------------------------
// <copyright file="AffLanguageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.7.31">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffLanguageCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_LanguageSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}