// --------------------------------------------------------------------------
// <copyright file="LanguageUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.Language
{
    using System.ComponentModel.DataAnnotations;
    using NAP.Framework.Data;

    /// <summary>
    /// LanguageUpdate Criteria class.
    /// </summary>
    public class LanguageUpdateCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        [Required]
        public string LanguageID { get; set; }

        [Required]
        public string LanguageName { get; set; }

        public string CountryID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_LanguageUpd";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(LanguageID)
                   && !string.IsNullOrEmpty(LanguageName)
                   && !string.IsNullOrEmpty(CountryID);
        }
    }
}