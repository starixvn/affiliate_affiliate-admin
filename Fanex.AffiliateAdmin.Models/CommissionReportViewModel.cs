// --------------------------------------------------------------------------
// <copyright file="CommissionReportViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class CommissionReportViewModel
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

        private readonly List<SelectListItem> _groupdByOptions = new List<SelectListItem>
                {
                    new SelectListItem()
                    {
                        Text = Lang.AffiliateMonth,
                        Value = "0",
                    },
                    new SelectListItem()
                    {
                        Text = Lang.Affiliate,
                        Value = "1",
                        Selected = true
                    }
                };

        private readonly IEnumerable<SelectListItem> _sortByList = new List<SelectListItem>
            {
                    ////new SelectListItem { Text = Lang.Commission, Value = "0" },
                     new SelectListItem { Text = Lang.ActiveMembers, Value = "1" },
                     new SelectListItem { Text = Lang.BetCount, Value = "2" },
                     new SelectListItem { Text = Lang.Deposit, Value = "3" },
                     new SelectListItem { Text = Lang.Withdrawal, Value = "4" },
                     new SelectListItem { Text = Lang.Turnover, Value = "5" },
                    new SelectListItem { Text = Lang.Winloss, Value = "6" },
                    new SelectListItem { Text = Lang.Affiliate, Value = "7" },
                    new SelectListItem { Text = Lang.Month, Value = "8" }
            };

        private SelectList _affTypeOptionList;

        private IList<SelectListItem> _baseCurrencyList;

        private SelectList _commissionGroupOptionList;

        private SelectList _currencyOptionList;

        private SelectList _preferLanguageOptionList;

        private SelectList _sortByOptionList;

        #endregion Fields

        public CommissionReportViewModel()
        {
            ExcludeTest = true;
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

                    return new SelectList(_baseCurrencyList, "Value", "Text");
                }

                return new SelectList(AppSettings.BaseCurrencyInAffiliate, "Value", "Text");
            }
        }

        public SelectList CommissionGroupOptionList
        {
            get
            {
                if (_commissionGroupOptionList == null)
                {
                    _commissionGroupOptionList = new SelectList(AppSettings.GetCommGroups(null), "GroupID", "GroupName");
                }

                return _commissionGroupOptionList;
            }
        }

        public IEnumerable<SelectListItem> CurrencyOptionList
        {
            get
            {
                if (_currencyOptionList == null)
                {
                    _currencyOptionList = new SelectList(SessionManager.CurrentSession.CurrencyList, "CurrencyID", "CurrencyCode");
                }

                return _currencyOptionList;
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

        public IEnumerable<SelectListItem> GroupByOptionList
        {
            get { return _groupdByOptions; }
        }

        public int GroupId { get; set; }

        public SelectList PreferLanguageOptionList
        {
            get
            {
                if (_preferLanguageOptionList == null)
                {
                    _preferLanguageOptionList = new SelectList(AppSettings.AffSupportLanguages, "LanguageID", "LanguageName");
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
                                         ? AppSettings.Products.Select(t => t.ProductID)
                                         : productList;

                return new MultiSelectList(AppSettings.Products, "ProductID", "ProductName", selectedValues);
            }
        }

        public string Products { get; set; }

        public SelectList SortByOptionList
        {
            get
            {
                if (_sortByOptionList == null)
                {
                    _sortByOptionList = new SelectList(_sortByList.OrderBy(t => t.Text), "Value", "Text", "7");
                }

                return _sortByOptionList;
            }
        }

        public string Status { get; set; }

        public DateTime? ToDate { get; set; }

        #region Methods

        public void AddBaseCurrencyList(IList<SelectListItem> baseCurrencyList)
        {
            _baseCurrencyList = baseCurrencyList;
        }

        #endregion Methods
    }
}