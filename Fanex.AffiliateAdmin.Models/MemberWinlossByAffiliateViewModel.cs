// --------------------------------------------------------------------------
// <copyright file="MemberWinlossByAffiliateViewModel.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.AffiliateAdmin.Core.Options;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class MemberWinlossByAffiliateViewModel
    {
        private readonly IList<SelectListItem> _groupByList = new List<SelectListItem>
        {
                new SelectListItem { Text = Lang.Date, Value = "0" },
                new SelectListItem { Text = Lang.Member, Value = "1" }
        };

        private IList<SelectListItem> _baseCurrencyList;

        private IEnumerable<SelectListItem> _playerCurrencyOptions;

        public MemberWinlossByAffiliateViewModel()
        {
            ExcludeTest = true;
        }

        public string Affiliate { get; set; }

        public int BaseCurrency { get; set; }

        public IList<SelectListItem> BaseCurrencyList
        {
            get
            {
                return _baseCurrencyList;
            }
        }

        public IEnumerable<SelectListItem> BaseCurrencyOptionList
        {
            get
            {
                if (_baseCurrencyList.Count == 2 && _baseCurrencyList[0].Text == _baseCurrencyList[1].Text)
                {
                    _baseCurrencyList.RemoveAt(1);
                }

                return new SelectList(_baseCurrencyList, "Value", "Text");
            }
        }

        public int DrawFilter { get; set; }

        public IEnumerable<SelectListItem> DrawFilterOptionList
        {
            get
            {
                return new SelectList(OptionManager.DrawFilters, "Value", "Text", DrawFilter.ToString());
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

        public IEnumerable<SelectListItem> PlayerCurrencyOptionList
        {
            get
            {
                if (_playerCurrencyOptions == null)
                {
                    _playerCurrencyOptions = AppSettings.PlayerCurrencies.Select(t => new SelectListItem
                        {
                            Text = t.CurrencyCode,
                            Value = t.CurrencyID.ToString()
                        }).ToList();
                }

                return _playerCurrencyOptions;
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