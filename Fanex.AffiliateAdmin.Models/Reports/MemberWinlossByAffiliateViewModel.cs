namespace Fanex.AffiliateAdmin.Models.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class MemberWinlossByAffiliateViewModel
    {
        public MemberWinlossByAffiliateViewModel()
        {
            ExcludeTest = true;
        }

        public string Affiliate { get; set; }

        public string PlayerName { get; set; }

        public byte BaseCurrency { get; set; }

        public IEnumerable<SelectListItem> BaseCurrencyOptionList { get; set; }

        public int DrawFilter { get; set; }

        public IEnumerable<SelectListItem> DrawFilterOptionList { get; set; }

        public bool ExcludeTest { get; set; }

        public int GroupBy { get; set; }

        public SelectList GroupByOptionList { get; set; }

        public int GroupId { get; set; }

        public string PlayerCurrencies { get; set; }

        public IEnumerable<SelectListItem> PlayerCurrencyOptionList { get; set; }

        public MultiSelectList ProductOptionList { get; set; }

        public string Products { get; set; }

        public string Status { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int ViewMode { get; set; }
    }
}