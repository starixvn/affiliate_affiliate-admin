// --------------------------------------------------------------------------
// <copyright file="AffiliateCommissionByAffiliateViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.3">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateCommissionByAffiliateViewModel
    {
        private readonly IList<SelectListItem> _transferStatusList = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.All, Value = "0" },
            new SelectListItem { Text = Lang.Processing, Value = "1" },
            new SelectListItem { Text = Lang.Transfered, Value = "2" }
        };

        private readonly IEnumerable<SelectListItem> _sortByList = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.Affiliate, Value = "0" },
            new SelectListItem { Text = Lang.Month, Value = "1" },
            new SelectListItem { Text = Lang.TotalPayment, Value = "2" }
        };

        private IList<SelectListItem> _baseCurrencyList;

        private SelectList _transferStatusOptions;

        private SelectList _sortByOptions;

        public AffiliateCommissionByAffiliateViewModel()
        {
            ExcludeTest = true;
            SortBy = 1;
        }

        [Display(Name = "BaseCurrency", ResourceType = typeof(Lang))]
        public string BaseCurrency { get; set; }

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
       
        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public int? Currency { get; set; }

        [Display(Name = "DateOption", ResourceType = typeof(Lang))]
        public short? DateOption { get; set; }

        [Display(Name = "ExcludeTestAccounts", ResourceType = typeof(Lang))]
        public bool ExcludeTest { get; set; }

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

                return new MultiSelectList(AppSettings.Products, "ProductID", "ProductName", selectedValues);
            }
        }

        public string Products { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

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

        public int SortBy { get; set; }

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

        public int ViewMode { get; set; }

        #region Methods

        public void AddBaseCurrencyList(IList<SelectListItem> baseCurrencyList)
        {
            _baseCurrencyList = baseCurrencyList;
        }

        #endregion Methods
    }
}