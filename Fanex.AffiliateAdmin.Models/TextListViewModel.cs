namespace Fanex.AffiliateAdmin.Models
{
    using Core.Configurations;
    using Resources;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TextListViewModel
    {
        #region Fields

        private IEnumerable<SelectListItem> _bannerCategoryOptions;

        private IEnumerable<SelectListItem> _memberSiteSupportedLanguageOptions;

        private IEnumerable<SelectListItem> _playerCurrencyOptions;

        #endregion

        [Display(Name = "Category", ResourceType = typeof(Lang))]
        public short? Category { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public short? Currency { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Lang))]
        public string Language { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        #region Option List

        public IEnumerable<SelectListItem> BannerCategoryOptionList
        {
            get
            {
                var selectItemList = new List<SelectListItem>();

                foreach (var item in MediaSettings.BannerCategories)
                {
                    var value = Lang.ResourceManager.GetString(item.Value.Replace(" ", string.Empty));

                    selectItemList.Add(new SelectListItem
                    {
                        Text = string.IsNullOrEmpty(value) ? item.Value : value,
                        Value = item.TypeId.ToString()
                    });
                }

                selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });
                _bannerCategoryOptions = selectItemList;

                return _bannerCategoryOptions;
            }
        }

        public IEnumerable<SelectListItem> MemberSiteSupportedLanguageOptionList
        {
            get
            {
                if (_memberSiteSupportedLanguageOptions == null)
                {
                    var selectItemList = AppSettings.MemberSiteSiteSupportedLanguages.Select(t => new SelectListItem
                    {
                        Text = t.LanguageName,
                        Value = t.LanguageID
                    }).ToList();

                    selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });
                    _memberSiteSupportedLanguageOptions = selectItemList;
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

                    if (selectItemList.Count > 1)
                    {
                        selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });
                    }

                    _playerCurrencyOptions = selectItemList;
                }

                return _playerCurrencyOptions;
            }
        }

        #endregion
    }
}