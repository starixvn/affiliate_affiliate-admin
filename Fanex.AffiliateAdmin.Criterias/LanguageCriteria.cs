// --------------------------------------------------------------------------
// <copyright file="LanguageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.14">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Language criteria class.
    /// </summary>
    public class LanguageCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public string Type { get; set; }

        /// <summary>
        /// Get store procedure name by key.
        /// </summary>
        /// <returns>Store procedure name.</returns>
        public override string GetSettingKey()
        {
            string settingKey = string.Empty;

            switch (Type)
            {
                case "Affiliate":
                    settingKey = "AffAdmin_AdminSupportedLanguagesSel";
                    break;
            }

            return settingKey;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}