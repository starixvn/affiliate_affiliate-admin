namespace Fanex.AffiliateAdmin.Models.Reports
{
    using System;
    using System.Web.Mvc;

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

        public SelectList GroupByOptionList { get; set; }

        public SelectList DrawFilterOptionList { get; set; }

        public SelectList BaseCurrencyOptionList { get; set; }

        public MultiSelectList ProductOptionList { get; set; }

        public int ViewMode { get; set; }
    }
}