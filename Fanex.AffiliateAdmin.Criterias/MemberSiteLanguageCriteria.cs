// --------------------------------------------------------------------------
// <copyright file="MemberSiteLanguageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.26">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Member Site Language Criteria.
    /// </summary>
    public class MemberSiteLanguageCriteria : CriteriaBase
    {
        public string Environment { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_MemberSupportedLanguagesSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}