namespace Fanex.AffiliateAdmin.Models
{
    using System;
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
    public class MemberWinlossViewModel
    {
        #region Fields

        private readonly IList<SelectListItem> _groupByList = new List<SelectListItem>
        {
                new SelectListItem { Text = Lang.Date, Value = "0" },
                new SelectListItem { Text = Lang.Member, Value = "1" }
        };

        #endregion Fields

        public MemberWinlossViewModel()
        {
            ExcludeTest = true;
        }

        public string AffName { get; set; }

        public string AffTypes { get; set; }

        public string CommGroups { get; set; }

        public string PreferLanguages { get; set; }

        public int DrawFilter { get; set; }

        public string Status { get; set; }

        public int GroupBy { get; set; }

        public int BaseCurrency { get; set; }

        public bool ExcludeTest { get; set; }

        public string PlayerCurrencies { get; set; }

        public string Products { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public MultiSelectList AffTypeOptionList
        {
            get
            {
                var affTypeList = string.IsNullOrEmpty(AffTypes)
                                       ? new List<byte>()
                                       : AffTypes.Split(',').Select(t => byte.Parse(t));

                var selectedValues = string.IsNullOrEmpty(AffTypes)
                                         ? AppSettings.AffTypes.Select(t => t.TypeID)
                                         : affTypeList;

                return new MultiSelectList(AppSettings.AffTypes, "TypeID", "Name", selectedValues);
            }
        }

        public MultiSelectList CommissionGroupOptionList
        {
            get
            {
                var groups = AppSettings.GetCommGroups();

                var commGroupList = string.IsNullOrEmpty(CommGroups)
                                       ? new List<int>()
                                       : CommGroups.Split(',').Select(t => int.Parse(t));

                var selectedValues = string.IsNullOrEmpty(CommGroups)
                                         ? groups.Select(t => t.GroupID)
                                         : commGroupList;

                return new MultiSelectList(groups, "GroupID", "GroupName", selectedValues);
            }
        }

        public MultiSelectList PreferLanguageOptionList
        {
            get
            {
                var preferLanguagesList = string.IsNullOrEmpty(PreferLanguages)
                                    ? new List<string>()
                                    : PreferLanguages.Split(',').ToList();

                var selectedValues = string.IsNullOrEmpty(PreferLanguages)
                                        ? AppSettings.AffSupportLanguages.Select(t => t.LanguageID)
                                        : preferLanguagesList;

                return new MultiSelectList(AppSettings.AffSupportLanguages, "LanguageID", "LanguageName", selectedValues);
            }
        }

        public MultiSelectList PlayerCurrencyOptionList
        {
            get
            {
                var currencyList = string.IsNullOrEmpty(PlayerCurrencies)
                                       ? new List<short>()
                                       : PlayerCurrencies.Split(',').Select(short.Parse);

                var selectedValues = string.IsNullOrEmpty(PlayerCurrencies)
                                         ? AppSettings.PlayerCurrencies.Select(t => t.CurrencyID)
                                         : currencyList;

                var selectItemList = new MultiSelectList(AppSettings.PlayerCurrencies, "CurrencyID", "CurrencyCode", selectedValues);

                return selectItemList;
            }
        }

        public SelectList GroupByOptionList
        {
            get
            {
                return new SelectList(_groupByList, "Value", "Text", GroupBy.ToString());
            }
        }

        public SelectList DrawFilterOptionList
        {
            get
            {
                return new SelectList(OptionManager.DrawFilters, "Value", "Text", DrawFilter.ToString());
            }
        }

        public SelectList BaseCurrencyOptionList
        {
            get
            {
                return new SelectList(AppSettings.BaseCurrencyInMember, "Value", "Text", BaseCurrency.ToString());
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

        public int ViewMode { get; set; }
    }
}