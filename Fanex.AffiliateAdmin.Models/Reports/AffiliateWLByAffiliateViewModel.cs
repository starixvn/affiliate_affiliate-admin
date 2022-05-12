namespace Fanex.AffiliateAdmin.Models.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Options;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class AffiliateWLByAffiliateViewModel
    {
        public AffiliateWLByAffiliateViewModel()
        {
            ExcludeTest = true;
            SortBy = 1;
        }

        public string Affiliate { get; set; }

        public byte BaseCurrency { get; set; }

        public int DateOption { get; set; }

        public int DrawFilter { get; set; }

        public bool ExcludeTest { get; set; }

        public int GroupId { get; set; }

        public string Products { get; set; }

        public int SortBy { get; set; }

        public string Status { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int ViewMode { get; set; }

        public IEnumerable<SelectListItem> AffTypeOptionList { get; set; }

        public SelectList BaseCurrencyOptionList { get; set; }

        public IEnumerable<SelectListItem> DrawFilterOptionList => new SelectList(OptionManager.DrawFilters, "Value", "Text", DrawFilter.ToString());

        public MultiSelectList ProductOptionList { get; set; }

        public SelectList SortByOptionList => new SelectList(OptionManager.SortByForAffiliateWinLoss.OrderBy(t => t.Text), "Value", "Text", SortBy);
    }
}