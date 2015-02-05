// --------------------------------------------------------------------------
// <copyright file="CountryAddCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.Country
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.Data.Repository;
    using Resources;

    /// <summary>
    /// CountryAdd Criteria class.
    /// </summary>
    public class CountryAddCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        [StringLength(6, ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ExceedCharacters")]
        [RegularExpression(@"^([a-zA-Z_]+)$", ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "InvalidID")]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "FieldRequired")]
        public string CountryID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "FieldRequired")]
        public string CountryName { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public short DisplayOrder { get; set; }

        public bool IsEnabled { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CountryIns";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(CountryID) && !string.IsNullOrEmpty(CountryName);
        }
    }
}