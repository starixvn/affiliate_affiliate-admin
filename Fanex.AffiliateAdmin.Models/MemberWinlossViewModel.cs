namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Options;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class MemberWinlossViewModel
    {
        public MemberWinlossViewModel()
        {
            ExcludeTest = true;
        }

        public string AffName { get; set; }

        public string AffTypes { get; set; }

        public string CommGroups { get; set; }

        public string PreferLanguages { get; set; }

        public int DrawFilter { get; set; }

        public string Status { get; set; }

        public int GroupBy { get; set; }

        public byte BaseCurrency { get; set; }

        public bool ExcludeTest { get; set; }

        public string PlayerCurrencies { get; set; }

        public string Products { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public MultiSelectList AffTypeOptionList { get; set; }

        public MultiSelectList CommissionGroupOptionList { get; set; }

        public MultiSelectList PreferLanguageOptionList { get; set; }

        public MultiSelectList PlayerCurrencyOptionList { get; set; }

        public SelectList GroupByOptionList => new SelectList(OptionManager.GroupByListForMemberWinloss, "Value", "Text", GroupBy.ToString());

        public SelectList DrawFilterOptionList => new SelectList(OptionManager.DrawFilters, "Value", "Text", DrawFilter.ToString());

        public SelectList BaseCurrencyOptionList { get; set; }

        public MultiSelectList ProductOptionList { get; set; }

        public int ViewMode { get; set; }
    }
}