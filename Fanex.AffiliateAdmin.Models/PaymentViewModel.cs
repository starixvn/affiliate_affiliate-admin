// --------------------------------------------------------------------------
// <copyright file="PaymentViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.21">Created</change>
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
    public class PaymentViewModel
    {
        [Display(Name = "Affiliate", ResourceType = typeof(Lang))]
        public string Username { get; set; }

        [Display(Name = "PreferLang", ResourceType = typeof(Lang))]
        public string PreferredLanguage { get; set; }

        [Display(Name = "AffType", ResourceType = typeof(Lang))]
        public short? AffType { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Lang))]
        public string Status { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public int? Currency { get; set; }

        [Display(Name = "DateOption", ResourceType = typeof(Lang))]
        public short? DateOption { get; set; }

        [Display(Name = "From", ResourceType = typeof(Lang))]
        public DateTime FromDate { get; set; }

        [Display(Name = "To", ResourceType = typeof(Lang))]
        public DateTime ToDate { get; set; }

        [Display(Name = "BaseCurrency", ResourceType = typeof(Lang))]
        public string BaseCurrency { get; set; }

        public SelectList BaseCurrencyListSelectItem
        {
            get
            {
                return new SelectList(AppSettings.BaseCurrencyInAffiliate, "Value", "Text");
            }
        }

        public List<SelectListItem> ListStatus
        {
            get
            {
                return PaymentSettings.ListStatus.Select(status => new SelectListItem
                {
                    Value = status.Value,
                    Text = status.Text,
                    Selected = status.Value == Status
                }).ToList();
            }
        }

        [Display(Name = "ExcludeTestAccounts", ResourceType = typeof(Lang))]
        public bool ExcludeTest { get; set; }

        public bool GetAll { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}