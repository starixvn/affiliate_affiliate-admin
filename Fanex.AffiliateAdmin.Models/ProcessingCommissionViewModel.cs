// --------------------------------------------------------------------------
// <copyright file="ProcessingCommissionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.21">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Session;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ProcessingCommissionViewModel
    {
        private MultiSelectList _affiliateCurrencyOptions;

        private MultiSelectList _affTypeOptions;

        private SelectList _baseCurrencyOptions;

        private MultiSelectList _commissionGroupOptions;

        private MultiSelectList _preferLanguageOptions;

        public ProcessingCommissionViewModel()
        {
            ExcludeTest = true;
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
                    var groups = AppSettings.GetCommGroups(null);

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
                                         ? AppSettings.Products.Select(t => t.ProductID)
                                         : productList;

                return new MultiSelectList(AppSettings.Products, "ProductID", "ProductName", selectedValues);
            }
        }

        public string Products { get; set; }

        [Display(Name = "Affiliate", ResourceType = typeof(Lang))]
        public string Username { get; set; }
    }
}