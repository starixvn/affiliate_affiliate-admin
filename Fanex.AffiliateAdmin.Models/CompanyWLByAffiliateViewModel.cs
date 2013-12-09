// --------------------------------------------------------------------------
// <copyright file="CompanyWLByAffiliateViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class CompanyWLByAffiliateViewModel
    {
        public CompanyWLByAffiliateViewModel()
        {
            ExcludeTest = true;
        }

        public string Affiliate { get; set; }

        public int GroupBy { get; set; }

        public int DrawFilter { get; set; }

        public string Status { get; set; }

        public int GroupId { get; set; }

        public bool ExcludeTest { get; set; }

        public MultiSelectList PlayerCurrencyOptionList
        {
            get
            {
                return new MultiSelectList(AppSettings.PlayerCurrencies, "CurrencyID", "CurrencyCode");
            }
        } 

        public SelectList GroupByOptionList
        {
            get
            {
                return new SelectList(AppSettings.GroupByDateMonth, "Value", "Text", GroupBy.ToString());
            }
        }

        public IEnumerable<SelectListItem> DrawFilterOptionList
        {
            get
            {
                return new SelectList(AppSettings.DrawFilters, "Value", "Text", DrawFilter.ToString());
            }
        }

        public IList<SelectListItem> BaseCurrencyList { get; set; }

        public SelectList BaseCurrencyOptionList
        {
            get
            {
                if (BaseCurrencyList.Count == 2 && BaseCurrencyList[0].Text == BaseCurrencyList[1].Text)
                {
                    BaseCurrencyList.RemoveAt(1);
                }

                return new SelectList(BaseCurrencyList, "Value", "Text");
            }
        }
    }
}