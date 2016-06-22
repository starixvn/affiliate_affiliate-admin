namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Options;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class CompanyWinlossViewModel
    {
        #region Fields

        private readonly IList<SelectListItem> _groupByList = new List<SelectListItem>
        {
                new SelectListItem { Text = Lang.Date, Value = "0" },
                new SelectListItem { Text = Lang.Month, Value = "1" }
        };

        private MultiSelectList _affTypeOptionList;

        private SelectList _baseCurrencyOptionList;

        private MultiSelectList _commissionGroupOptionList;

        private MultiSelectList _preferLanguageOptionList;

        private MultiSelectList _playerCurrencyOptionList;

        #endregion Fields

        public CompanyWinlossViewModel()
        {
            ExcludeTest = true;
        }

        public string Affiliate { get; set; }

        public MultiSelectList AffTypeOptionList
        {
            get
            {
                if (_affTypeOptionList == null)
                {
                    _affTypeOptionList = new MultiSelectList(AppSettings.AffTypes, "TypeID", "Name");
                }

                return _affTypeOptionList;
            }
        }

        public SelectList BaseCurrencyOptionList
        {
            get
            {
                if (_baseCurrencyOptionList == null)
                {
                    _baseCurrencyOptionList = new SelectList(AppSettings.BaseCurrencyInMember, "Value", "Text");
                }

                return _baseCurrencyOptionList;
            }
        }

        public MultiSelectList CommissionGroupOptionList
        {
            get
            {
                if (_commissionGroupOptionList == null)
                {
                    _commissionGroupOptionList = new MultiSelectList(AppSettings.GetCommGroups(null), "GroupID", "GroupName");
                }

                return _commissionGroupOptionList;
            }
        }

        public int DateOption { get; set; }

        public int DrawFilter { get; set; }

        public SelectList DrawFilterOptionList
        {
            get
            {
                return new SelectList(OptionManager.DrawFilters, "Value", "Text", DrawFilter.ToString());
            }
        }

        public bool ExcludeTest { get; set; }

        public SelectList GroupByOptionList
        {
            get
            {
                return new SelectList(_groupByList, "Value", "Text", DateOption.ToString());
            }
        }

        public int GroupId { get; set; }

        public MultiSelectList PlayerCurrencyOptionList
        {
            get
            {
                if (_playerCurrencyOptionList == null)
                {
                    _playerCurrencyOptionList = new MultiSelectList(AppSettings.PlayerCurrencies, "CurrencyID", "CurrencyCode");
                }

                return _playerCurrencyOptionList;
            }
        }

        public MultiSelectList PreferLanguageOptionList
        {
            get
            {
                if (_preferLanguageOptionList == null)
                {
                    _preferLanguageOptionList = new MultiSelectList(AppSettings.AffSupportLanguages, "LanguageID", "LanguageName");
                }

                return _preferLanguageOptionList;
            }
        }

        public MultiSelectList ProductOptionList
        {
            get
            {
                var productList = string.IsNullOrEmpty(Products)
                                      ? new List<byte>()
                                      : Products.Split(',').Select(byte.Parse);

                var selectedValues = string.IsNullOrEmpty(Products)
                                         ? AppSettings.Products.Where(t => t.IsEnabled).Select(t => t.ProductID)
                                         : productList;

                var translatedProducts = new List<Product>();

                foreach (var product in AppSettings.Products)
                {
                    var name = Lang.ResourceManager.GetString(product.ProductName.Replace(" ", string.Empty));

                    translatedProducts.Add(new Product
                    {
                        ProductID = product.ProductID,
                        ProductName = string.IsNullOrEmpty(name) ? product.ProductName : name
                    });
                }

                return new MultiSelectList(translatedProducts, "ProductID", "ProductName", selectedValues);
            }
        }

        public string Products { get; set; }

        public string Status { get; set; }
    }
}