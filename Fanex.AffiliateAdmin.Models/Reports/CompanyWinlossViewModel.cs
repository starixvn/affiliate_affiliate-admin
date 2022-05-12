namespace Fanex.AffiliateAdmin.Models.Reports
{
    using System.Web.Mvc;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class CompanyWinlossViewModel
    {
        public CompanyWinlossViewModel()
        {
            ExcludeTest = true;
        }

        public MultiSelectList AffTypeOptionList { get; set; }

        public SelectList BaseCurrencyOptionList { get; set; }

        public MultiSelectList CommissionGroupOptionList { get; set; }

        public int DateOption { get; set; }

        public int DrawFilter { get; set; }

        public SelectList DrawFilterOptionList { get; set; }
        public bool ExcludeTest { get; set; }

        public SelectList GroupByOptionList { get; set; }

        public MultiSelectList PlayerCurrencyOptionList { get; set; }

        public MultiSelectList PreferLanguageOptionList { get; set; }

        public MultiSelectList ProductOptionList { get; set; }
    }
}