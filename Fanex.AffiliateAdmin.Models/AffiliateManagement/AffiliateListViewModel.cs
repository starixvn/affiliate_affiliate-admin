namespace Fanex.AffiliateAdmin.Models.AffiliateManagement
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Options;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class AffiliateListViewModel
    {
        public AffiliateListViewModel()
        {
            ExcludeTest = true;
        }

        public string Affiliate { get; set; }

        public int DateOption { get; set; }

        public bool IsFavorite { get; set; }

        public string Status { get; set; }

        public int GroupId { get; set; }

        public bool ExcludeTest { get; set; } = true;

        public SelectList DateOptionList => new SelectList(OptionManager.DateOptions, "Value", "Text");

        public SelectList BaseCurrencyOptionList { get; set; }

        public IEnumerable<SelectListItem> SortByOptionList => OptionManager.SortByForAffiliateList;

        public IEnumerable<SelectListItem> StatusOptionList { get; set; }

        public IEnumerable<SelectListItem> CommissionGroupOptionList { get; set; }
    }
}
