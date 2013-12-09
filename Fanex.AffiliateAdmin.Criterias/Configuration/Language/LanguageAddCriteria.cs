// --------------------------------------------------------------------------
// <copyright file="LanguageAddCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// LanguageAdd Criteria class.
    /// </summary>
    public class LanguageAddCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public string LanguageID { get; set; }

        public string LanguageName { get; set; }

        public string CountryID { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_LanguageIns";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(LanguageID);
        }
    }
}