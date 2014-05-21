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

        private readonly IEnumerable<SelectListItem> _sortByList = new List<SelectListItem>
            {
                    new SelectListItem { Text = Lang.Commission, Value = "0" },
                     new SelectListItem { Text = Lang.ActiveMembers, Value = "1" },
                     new SelectListItem { Text = Lang.BetCount, Value = "2" },
                     new SelectListItem { Text = Lang.Deposit, Value = "3" },
                     new SelectListItem { Text = Lang.Withdrawal, Value = "4" },
                     new SelectListItem { Text = Lang.Turnover, Value = "5" },
                    new SelectListItem { Text = Lang.Winloss, Value = "6" }
            };

        private SelectList _commissionGroupOptionList;

        private SelectList _preferLanguageOptionList;

        private SelectList _affTypeOptionList;

        private SelectList _currencyOptionList;

        private SelectList _sortByOptionList;

        #endregion

        public CommissionReportViewModel()
        {
            ExcludeTest = true;
        }

        public string Affiliate { get; set; }

        public int DateOption { get; set; }

        public int DrawFilter { get; set; }

        public string Status { get; set; }

        public int GroupId { get; set; }

        public bool ExcludeTest { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string Products { get; set; }

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

        public IEnumerable<SelectListItem> GroupByOptionList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem()
                    {
                        Text = Lang.Month,
                        Value = "0",
                        Selected = true
                    },
                    new SelectListItem()
                    {
                        Text = Lang.Affiliate,
                        Value = "1"
                    }
                };                
            }
        }

        public IEnumerable<SelectListItem> DrawFilterOptionList
        {
            get
            {
                return new List<SelectListItem>
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
            }
        }

        public IList<SelectListItem> BaseCurrencyList { get; set; }

        public SelectList BaseCurrencyOptionList
        {
            get
            {
                if (BaseCurrencyList != null)
                {
                    if (BaseCurrencyList.Count == 2 && BaseCurrencyList[0].Text == BaseCurrencyList[1].Text)
                    {
                        BaseCurrencyList.RemoveAt(1);
                    }

                    return new SelectList(BaseCurrencyList, "Value", "Text");
                }

                return new SelectList(AppSettings.BaseCurrencyInAffiliate, "Value", "Text");
            }
        }

        public SelectList SortByOptionList
        {
            get
            {
                if (_sortByOptionList == null)
                {
                    _sortByOptionList = new SelectList(_sortByList.OrderBy(t => t.Text), "Value", "Text", "0");
                }

                return _sortByOptionList;
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
    }
} 