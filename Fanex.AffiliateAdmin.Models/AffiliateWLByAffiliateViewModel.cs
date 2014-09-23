// --------------------------------------------------------------------------
// <copyright file="AffiliateWLByAffiliateViewModel.cs" company="Nexcel Solutions Vietnam">
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
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class AffiliateWLByAffiliateViewModel
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

        private readonly IEnumerable<SelectListItem> _sortByList = new List<SelectListItem>
            {
                     new SelectListItem { Text = Lang.ActiveMembers, Value = "0" },
                     new SelectListItem { Text = Lang.Affiliate, Value = "1" },
                     new SelectListItem { Text = Lang.BetCount, Value = "2" },
                     new SelectListItem { Text = Lang.PlayerComm, Value = "3" },
                     new SelectListItem { Text = Lang.Deposit, Value = "4" },
                     new SelectListItem { Text = Lang.Month, Value = "5" },
                     new SelectListItem { Text = Lang.Turnover, Value = "6" },
                     new SelectListItem { Text = Lang.Winloss, Value = "7" },
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
                return _drawFilterOptions;
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

                return new MultiSelectList(AppSettings.Products, "ProductID", "ProductName", selectedValues);
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