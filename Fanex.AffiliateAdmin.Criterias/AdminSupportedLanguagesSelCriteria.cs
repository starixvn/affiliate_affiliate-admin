// --------------------------------------------------------------------------
// <copyright file="AdminSupportedLanguagesSelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.2.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Language criteria class.
    /// </summary>
    public class AdminSupportedLanguagesSelCriteria : CriteriaBase
    {
        /// <summary>
        /// Get store procedure name by key.
        /// </summary>
        /// <returns>Store procedure name.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_AdminSupportedLanguagesSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}