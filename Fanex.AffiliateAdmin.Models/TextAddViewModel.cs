// --------------------------------------------------------------------------
// <copyright file="TextAddViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.30">Created</change>
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
    public class TextAddViewModel
    {
        #region Fields
        
        private IEnumerable<SelectListItem> _bannerCategoryOptions;

        private IEnumerable<SelectListItem> _memberSiteSupportedLanguageOptions;

        private IEnumerable<SelectListItem> _playerCurrencyOptions; 

        #endregion

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "CategoryFieldIsRequired")]
        [Display(Name = "Category", ResourceType = typeof(Lang))]
        public short Category { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "TextBoxFieldIsRequired")]
        [Display(Name = "Text", ResourceType = typeof(Lang))]
        public string Content { get; set; }
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "CurrencyFieldIsRequired")]
        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public short Currency { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Lang))]
        public string Language { get; set; }
        #region Option List

        public IEnumerable<SelectListItem> BannerCategoryOptionList
        {
            get
            {
                if (_bannerCategoryOptions == null)
                {
                    var selectItemList = MediaSettings.BannerCategories.Select(t => new SelectListItem
                        {
                            Text = t.Value,
                            Value = t.TypeId.ToString(),
                        }).ToList();

                    selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });
                    _bannerCategoryOptions = selectItemList;
                }

                return _bannerCategoryOptions;
            }
        }

        public IEnumerable<SelectListItem> MemberSiteSupportedLanguageOptionList
        {
            get
            {
                if (_memberSiteSupportedLanguageOptions == null)
                {
                    _memberSiteSupportedLanguageOptions = AppSettings.MemberSiteSiteSupportedLanguages.Select(t => new SelectListItem
                        {
                            Text = t.LanguageName,
                            Value = t.LanguageID
                        }).ToList();
                }

                return _memberSiteSupportedLanguageOptions;
            }
        }

        public IEnumerable<SelectListItem> PlayerCurrencyOptionList
        {
            get
            {
                if (_playerCurrencyOptions == null)
                {
                    var selectItemList = AppSettings.PlayerCurrencies.Select(t => new SelectListItem
                        {
                            Text = t.CurrencyCode,
                            Value = t.CurrencyID.ToString()
                        }).ToList();

                    selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });

                    _playerCurrencyOptions = selectItemList;
                }

                return _playerCurrencyOptions;
            }
        }

        #endregion
    }
}