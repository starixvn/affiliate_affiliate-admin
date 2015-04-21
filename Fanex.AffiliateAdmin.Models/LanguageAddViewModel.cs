// --------------------------------------------------------------------------
// <copyright file="LanguageAddViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.10">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using DTO;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LanguageAddViewModel
    {
        [StringLength(8, ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ExceedCharacters")]
        [RegularExpression(@"^([a-zA-Z-]+)$", ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "InvalidID")]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "IDFieldIsRequired")]
        public string LanguageID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "NameFieldIsRequired")]
        public string LanguageName { get; set; }

        public IEnumerable<DTO.Country> CountryList { get; set; }

        public IEnumerable<SelectListItem> CountryOptionList
        {
            get
            {
                var selectItemList = CountryList.Select(t => new SelectListItem
                {
                    Text = t.CountryName,
                    Value = t.CountryID
                }).ToList();

                return selectItemList;
            }
        }
    }
}