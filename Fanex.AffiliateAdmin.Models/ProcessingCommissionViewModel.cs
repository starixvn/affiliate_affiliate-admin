// --------------------------------------------------------------------------
// <copyright file="ProcessingCommissionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.21">Created</change>
// </history>

using System.Collections.Generic;

namespace Fanex.AffiliateAdmin.Models
{
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
        public ProcessingCommissionViewModel()
        {
            ExcludeTest = true;
        }

        [Display(Name = "Affiliate", ResourceType = typeof(Lang))]
        public string Username { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Lang))]
        public string Language { get; set; }

        [Display(Name = "AffType", ResourceType = typeof(Lang))]
        public short? AffType { get; set; }

        [Display(Name = "CommGroup", ResourceType = typeof(Lang))]
        public string CommGroup { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public int? Currency { get; set; }

        [Display(Name = "DateOption", ResourceType = typeof(Lang))]
        public short? DateOption { get; set; }

        [Display(Name = "BaseCurrency", ResourceType = typeof(Lang))]
        public string BaseCurrency { get; set; }

        public string Products { get; set; }

        public MultiSelectList AffTypeOptionList
        {
            get
            {
                return new MultiSelectList(AppSettings.AffTypes, "TypeID", "Name", AppSettings.AffTypes.Select(t => t.TypeID));
            }
        }

        public MultiSelectList CommissionGroupOptionList
        {
            get
            {
                var groups = AppSettings.GetCommGroups(null);

                return new MultiSelectList(groups, "GroupID", "GroupName");
            }
        }

        public MultiSelectList PreferLanguageOptionList
        {
            get
            {
                return new MultiSelectList(AppSettings.AffSupportLanguages, "LanguageID", "LanguageName");
            }
        }

        public MultiSelectList AffiliateCurrencyOptionList
        {
            get
            {
                return new MultiSelectList(SessionManager.CurrentSession.CurrencyList, "CurrencyID", "CurrencyCode");
            }
        }

        public SelectList BaseCurrencyOptionList
        {
            get
            {
                return new SelectList(AppSettings.BaseCurrencyInAffiliate, "Value", "Text");
            }
        }

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
                                         ? AppSettings.Products.Select(t => t.ProductID)
                                         : productList;

                return new MultiSelectList(AppSettings.Products, "ProductID", "ProductName", selectedValues);
            }
        }
    }
}