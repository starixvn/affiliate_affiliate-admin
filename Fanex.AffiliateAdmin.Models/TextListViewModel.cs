// --------------------------------------------------------------------------
// <copyright file="TextListViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.28">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TextListViewModel
    {
        [Display(Name = "Category", ResourceType = typeof(Lang))]
        public short? Category { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Lang))]
        public string Language { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public short? Currency { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        #region Option List

        public IEnumerable<SelectListItem> BannerCategoryOptionList
        {
            get
            {
                var selectItemList = MediaSettings.BannerCategories.Select(t => new SelectListItem
                {
                    Text = t.Value,
                    Value = t.TypeId.ToString()
                }).ToList();

                selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });

                return selectItemList;
            }
        }

        public IEnumerable<SelectListItem> MemberSiteSupportedLanguageOptionList
        {
            get
            {
                var selectItemList = AppSettings.MemberSiteSiteSupportedLanguages.Select(t => new SelectListItem
                {
                    Text = t.LanguageName,
                    Value = t.LanguageID
                }).ToList();

                selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });

                return selectItemList;
            }
        }

        public IEnumerable<SelectListItem> PlayerCurrencyOptionList
        {
            get
            {
                var selectItemList = AppSettings.PlayerCurrencies.Select(t => new SelectListItem
                {
                    Text = t.CurrencyCode,
                    Value = t.CurrencyID.ToString()
                }).ToList();

                selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });

                return selectItemList;
            }
        } 

        #endregion
    }
}