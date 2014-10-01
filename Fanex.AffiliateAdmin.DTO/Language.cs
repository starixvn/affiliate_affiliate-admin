// --------------------------------------------------------------------------
// <copyright file="Language.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.14">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Language class.
    /// </summary>
    public class Language
    {
        public string LanguageID { get; set; }

        public string LanguageName { get; set; }

        [ScriptIgnore]
        public string CountryID { get; set; }

        public string CountryName { get; set; }

        public int Creator { get; set; }

        public string CreatorName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.ToString(DateFormatManager.DateFormat);
            }
        }

        public short? Order { get; set; }
    }
}