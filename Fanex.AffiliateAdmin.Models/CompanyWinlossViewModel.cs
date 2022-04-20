namespace Fanex.AffiliateAdmin.Models
{
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Options;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class CompanyWinlossViewModel
    {
        public CompanyWinlossViewModel()
        {
            ExcludeTest = true;
        }

        public string Affiliate { get; set; }

        public MultiSelectList AffTypeOptionList { get; set; }

        public SelectList BaseCurrencyOptionList { get; set; }

        public MultiSelectList CommissionGroupOptionList { get; set; }

        public int DateOption { get; set; }

        public int DrawFilter { get; set; }

        public SelectList DrawFilterOptionList => new SelectList(OptionManager.DrawFilters, "Value", "Text", DrawFilter.ToString());
        public bool ExcludeTest { get; set; }

        public SelectList GroupByOptionList => new SelectList(OptionManager.GroupByListForCompanyWinloss, "Value", "Text", DateOption.ToString());

        public int GroupId { get; set; }

        public MultiSelectList PlayerCurrencyOptionList { get; set; }

        public MultiSelectList PreferLanguageOptionList { get; set; }

        public MultiSelectList ProductOptionList { get; set; }

        public string Products { get; set; }

        public string Status { get; set; }
    }
}