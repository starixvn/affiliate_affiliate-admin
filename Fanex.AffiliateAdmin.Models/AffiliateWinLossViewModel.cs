namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class AffiliateWinLossViewModel
    {
        #region Fields

        private readonly List<SelectListItem> _drawFilterOptions = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = Lang.Normal,
                        Value = "0"
                    },
                    new SelectListItem
                    {
                        Text = Lang.NoDraw,
                        Value = "1"
                    },
                    new SelectListItem
                    {
                        Text = Lang.NoDrawForTurnover,
                        Value = "2"
                    }
                };

        private readonly IList<SelectListItem> _groupByList = new List<SelectListItem>
                {
                    new SelectListItem { Text = Lang.Affiliate, Value = "0" },
                    new SelectListItem { Text = Lang.AffiliateMonth, Value = "1" }
                };

        private readonly IEnumerable<SelectListItem> _sortByList = new List<SelectListItem>
            {
                     new SelectListItem { Text = Lang.ActiveMembers, Value = "0" },
                     new SelectListItem { Text = Lang.Affiliate, Value = "1" },
                     new SelectListItem { Text = Lang.BetCount, Value = "2" },
                     new SelectListItem { Text = Lang.MemberComm, Value = "3" },
                     new SelectListItem { Text = Lang.Deposit, Value = "4" },
                     new SelectListItem { Text = Lang.Month, Value = "5" },
                     new SelectListItem { Text = Lang.Turnover, Value = "6" },
                     new SelectListItem { Text = Lang.AffiliateWinLoss, Value = "7" },
                     new SelectListItem { Text = Lang.Withdrawal, Value = "8" }
            };

        private IList<SelectListItem> _baseCurrencyList;

        private SelectList _sortByOptionList;

        #endregion Fields

        public AffiliateWinLossViewModel()
        {
            ExcludeTest = true;
            GroupBy = 1;
        }

        public string AffName { get; set; }

        public string AffTypes { get; set; }

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

        public IList<SelectListItem> BaseCurrencyList
        {
            get
            {
                return _baseCurrencyList;
            }
        }

        public int BaseCurrency { get; set; }

        public SelectList BaseCurrencyOptionList
        {
            get
            {
                if (_baseCurrencyList != null)
                {
                    if (_baseCurrencyList.Count == 2 && _baseCurrencyList[0].Text == _baseCurrencyList[1].Text)
                    {
                        _baseCurrencyList.RemoveAt(1);
                    }

                    return new SelectList(_baseCurrencyList, "Value", "Text", BaseCurrency);
                }

                return new SelectList(AppSettings.BaseCurrencyInAffiliate, "Value", "Text", BaseCurrency);
            }
        }

        public string CommGroups { get; set; }

        public MultiSelectList CommissionGroupOptionList
        {
            get
            {
                var groups = AppSettings.GetCommGroups(null);

                var commGroupList = string.IsNullOrEmpty(CommGroups)
                                       ? new List<int>()
                                       : CommGroups.Split(',').Select(t => int.Parse(t));

                var selectedValues = string.IsNullOrEmpty(CommGroups)
                                         ? groups.Select(t => t.GroupID)
                                         : commGroupList;

                return new MultiSelectList(groups, "GroupID", "GroupName", selectedValues);
            }
        }

        public string Currencies { get; set; }

        public IEnumerable<SelectListItem> CurrencyOptionList
        {
            get
            {
                var currencyList = string.IsNullOrEmpty(Currencies)
                                       ? new List<short>()
                                       : Currencies.Split(',').Select(short.Parse);

                var selectedValues = string.IsNullOrEmpty(Currencies)
                                         ? SessionManager.CurrentSession.CurrencyList.Select(t => t.CurrencyID)
                                         : currencyList;

                var selectItemList = new MultiSelectList(SessionManager.CurrentSession.CurrencyList, "CurrencyID", "CurrencyCode", selectedValues);

                return selectItemList;
            }
        }

        public int DateOption { get; set; }

        public int DrawFilter { get; set; }

        public IEnumerable<SelectListItem> DrawFilterOptionList
        {
            get
            {
                return _drawFilterOptions;
            }
        }

        public bool ExcludeTest { get; set; }

        public DateTime? FromDate { get; set; }

        public int GroupBy { get; set; }

        public SelectList GroupByOptionList
        {
            get
            {
                return new SelectList(_groupByList, "Value", "Text", GroupBy.ToString());
            }
        }

        public int GroupId { get; set; }

        public string PreferLanguages { get; set; }

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

        public string Products { get; set; }

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

        public SelectList SortByOptionList
        {
            get
            {
                if (_sortByOptionList == null)
                {
                    _sortByOptionList = new SelectList(_sortByList.OrderBy(t => t.Text), "Value", "Text", "3");
                }

                return _sortByOptionList;
            }
        }

        public string Status { get; set; }

        public DateTime? ToDate { get; set; }

        public int ViewMode { get; set; }

        #region Methods

        public void AddBaseCurrencyList(IList<SelectListItem> baseCurrencyList)
        {
            _baseCurrencyList = baseCurrencyList;
        }

        #endregion Methods
    }
}