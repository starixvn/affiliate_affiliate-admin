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
    public class AffiliateWLByAffiliateViewModel
    {
        #region Fields

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

        private SelectList _affTypeOptionList;

        private IList<SelectListItem> _baseCurrencyList;

        private SelectList _sortByOptionList;

        #endregion Fields

        public AffiliateWLByAffiliateViewModel()
        {
            ExcludeTest = true;
            SortBy = 1;
        }

        public string Affiliate { get; set; }

        public IEnumerable<SelectListItem> AffTypeOptionList
        {
            get
            {
                if (_affTypeOptionList == null)
                {
                    _affTypeOptionList = new SelectList(AppSettings.AffTypes, "TypeID", "Name");
                }

                return _affTypeOptionList;
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

                    return new SelectList(_baseCurrencyList, "Value", "Text", BaseCurrency.ToString());
                }

                return new SelectList(AppSettings.BaseCurrencyInAffiliate, "Value", "Text", BaseCurrency.ToString());
            }
        }

        public int DateOption { get; set; }

        public int DrawFilter { get; set; }

        public IEnumerable<SelectListItem> DrawFilterOptionList
        {
            get
            {
                return new SelectList(OptionManager.DrawFilters, "Value", "Text", DrawFilter.ToString());
            }
        }

        public bool ExcludeTest { get; set; }

        public int GroupId { get; set; }

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

        public int SortBy { get; set; }

        public SelectList SortByOptionList
        {
            get
            {
                if (_sortByOptionList == null)
                {
                    _sortByOptionList = new SelectList(_sortByList.OrderBy(t => t.Text), "Value", "Text", SortBy);
                }

                return _sortByOptionList;
            }
        }

        public string Status { get; set; }

        public DateTime? FromDate { get; set; }

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