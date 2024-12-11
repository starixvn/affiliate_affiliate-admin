namespace Fanex.AffiliateAdmin.Models
{
    using Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.DTO;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateCommissionViewModel
    {
        private readonly IList<SelectListItem> _transferStatusList = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.All, Value = "0" },
            new SelectListItem { Text = Lang.Processing, Value = "1" },
            new SelectListItem { Text = Lang.Transfered, Value = "2" }
        };

        private readonly IList<SelectListItem> _groupByList = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.Affiliate, Value = "0" },
            new SelectListItem { Text = Lang.AffiliateMonth, Value = "1" }
        };

        private readonly IEnumerable<SelectListItem> _sortByList = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.Affiliate, Value = "0" },
            new SelectListItem { Text = Lang.Month, Value = "1" },
            new SelectListItem { Text = Lang.TotalPayment, Value = "2" }
        };

        private MultiSelectList _affiliateCurrencyOptions;

        private MultiSelectList _affTypeOptions;

        private SelectList _baseCurrencyOptions;

        private MultiSelectList _commissionGroupOptions;

        private MultiSelectList _preferLanguageOptions;

        private SelectList _transferStatusOptions;

        private SelectList _groupByOptions;

        private SelectList _sortByOptions;

        public AffiliateCommissionViewModel()
        {
            ExcludeTest = true;
            GroupBy = 1;
            SortBy = 1;
        }

        public MultiSelectList AffiliateCurrencyOptionList
        {
            get
            {
                if (_affiliateCurrencyOptions == null)
                {
                    _affiliateCurrencyOptions = new MultiSelectList(SessionManager.CurrentSession.CurrencyList, "CurrencyID", "CurrencyCode");
                }

                return _affiliateCurrencyOptions;
            }
        }

        [Display(Name = "AffType", ResourceType = typeof(Lang))]
        public short? AffType { get; set; }

        public MultiSelectList AffTypeOptionList
        {
            get
            {
                if (_affTypeOptions == null)
                {
                    _affTypeOptions = new MultiSelectList(AppSettings.AffTypes, "TypeID", "Name", AppSettings.AffTypes.Select(t => t.TypeID));
                }

                return _affTypeOptions;
            }
        }

        [Display(Name = "BaseCurrency", ResourceType = typeof(Lang))]
        public string BaseCurrency { get; set; }

        public SelectList BaseCurrencyOptionList
        {
            get
            {
                if (_baseCurrencyOptions == null)
                {
                    _baseCurrencyOptions = new SelectList(AppSettings.BaseCurrencyInAffiliate, "Value", "Text");
                }

                return _baseCurrencyOptions;
            }
        }

        [Display(Name = "CommGroup", ResourceType = typeof(Lang))]
        public string CommGroup { get; set; }

        public MultiSelectList CommissionGroupOptionList
        {
            get
            {
                if (_commissionGroupOptions == null)
                {
                    var groups = AppSettings.GetCommGroups();

                    _commissionGroupOptions = new MultiSelectList(groups, "GroupID", "GroupName");
                }

                return _commissionGroupOptions;
            }
        }

        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public int? Currency { get; set; }

        [Display(Name = "DateOption", ResourceType = typeof(Lang))]
        public short? DateOption { get; set; }

        [Display(Name = "ExcludeTestAccounts", ResourceType = typeof(Lang))]
        public bool ExcludeTest { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Lang))]
        public string Language { get; set; }

        public MultiSelectList PreferLanguageOptionList
        {
            get
            {
                if (_preferLanguageOptions == null)
                {
                    _preferLanguageOptions = new MultiSelectList(AppSettings.AffSupportLanguages, "LanguageID", "LanguageName");
                }

                return _preferLanguageOptions;
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

        [Display(Name = "Affiliate", ResourceType = typeof(Lang))]
        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [Display(Name = "TransferState", ResourceType = typeof(Lang))]
        public byte TransferStatus { get; set; }

        public SelectList TransferStatusOptionList
        {
            get
            {
                if (_transferStatusOptions == null)
                {
                    _transferStatusOptions = new SelectList(_transferStatusList, "Value", "Text", TransferStatus);
                }

                return _transferStatusOptions;
            }
        }

        [Display(Name = "GroupBy", ResourceType = typeof(Lang))]
        public byte GroupBy { get; set; }

        public SelectList GroupByOptionList
        {
            get
            {
                if (_groupByOptions == null)
                {
                    _groupByOptions = new SelectList(_groupByList, "Value", "Text", GroupBy);
                }

                return _groupByOptions;
            }
        }

        public byte SortBy { get; set; }

        public SelectList SortByOptionList
        {
            get
            {
                if (_sortByOptions == null)
                {
                    _sortByOptions = new SelectList(_sortByList.OrderBy(t => t.Text), "Value", "Text", SortBy);
                }

                return _sortByOptions;
            }
        }
    }
}