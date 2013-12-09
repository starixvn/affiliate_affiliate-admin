// --------------------------------------------------------------------------
// <copyright file="LanguageDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.Language
{
    using NAP.Framework.Data;

    /// <summary>
    /// LanguageDetail Criteria class.
    /// </summary>
    public class LanguageDetailCriteria : CriteriaBase
    {
        public string ID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_LanguageSelByID";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ID);
        }
    }
}