namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Options;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class AffiliateWinLossViewModel
    {
        public AffiliateWinLossViewModel()
        {
            ExcludeTest = true;
            GroupBy = 1;
        }

        public string AffName { get; set; }

        public string AffTypes { get; set; }

        public MultiSelectList AffTypeOptionList { get; set; }

        public byte BaseCurrency { get; set; }

        public SelectList BaseCurrencyOptionList { get; set; }

        public string CommGroups { get; set; }

        public MultiSelectList CommissionGroupOptionList { get; set; }

        public string Currencies { get; set; }

        public IEnumerable<SelectListItem> CurrencyOptionList { get; set; }

        public int DateOption { get; set; }

        public int DrawFilter { get; set; }

        public IEnumerable<SelectListItem> DrawFilterOptionList => OptionManager.DrawFilters;

        public bool ExcludeTest { get; set; }

        public DateTime? FromDate { get; set; }

        public int GroupBy { get; set; }

        public SelectList GroupByOptionList => new SelectList(OptionManager.GroupByList, "Value", "Text", GroupBy.ToString());

        public int GroupId { get; set; }

        public string PreferLanguages { get; set; }

        public MultiSelectList PreferLanguageOptionList { get; set; }

        public string Products { get; set; }

        public MultiSelectList ProductOptionList { get; set; }

        public SelectList SortByOptionList =>  new SelectList(OptionManager.SortByForAffiliateWinLoss.OrderBy(t => t.Text), "Value", "Text", "3");

        public string Status { get; set; }

        public DateTime? ToDate { get; set; }

        public int ViewMode { get; set; }
    }
}