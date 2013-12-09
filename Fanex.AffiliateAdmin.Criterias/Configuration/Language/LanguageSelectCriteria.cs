// --------------------------------------------------------------------------
// <copyright file="LanguageSelectCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// LanguageSelect Criteria class.
    /// </summary>
    public class LanguageSelectCriteria : CriteriaBase
    {
        public string Name { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_LanguageSelAll";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}