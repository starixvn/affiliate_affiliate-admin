namespace Fanex.AffiliateAdmin.Models.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class CompanyWLByAffiliateViewModel
    {
        public byte BaseCurrency { get; set; }

        public SelectList BaseCurrencyOptionList { get; set; }

        public int DrawFilter { get; set; }

        public IEnumerable<SelectListItem> DrawFilterOptionList { get; set; }

        public int GroupBy { get; set; }

        public SelectList GroupByOptionList { get; set; }

        public string PlayerCurrencies { get; set; }

        public MultiSelectList PlayerCurrencyOptionList { get; set; }

        public MultiSelectList ProductOptionList { get; set; }

        public string Products { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int ViewMode { get; set; }
    }
}