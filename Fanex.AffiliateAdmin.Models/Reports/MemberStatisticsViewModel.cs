namespace Fanex.AffiliateAdmin.Models.Reports
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Core.Options;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberStatisticsViewModel
    {
        public MemberStatisticsViewModel()
        {
            ExcludeTest = true;
        }

        public bool ExcludeTest { get; set; }

        public IEnumerable<SelectListItem> SortByOptionList => OptionManager.SortByForMemberStatistics;

        public IEnumerable<SelectListItem> GroupByOptionList => OptionManager.GroupByForMemberStatistics;

        public IEnumerable<SelectListItem> PreferLanguageOptionList { get; set; }

        public IEnumerable<SelectListItem> AffiliateTypeOptionList { get; set; }

        public IEnumerable<SelectListItem> CurrencyOptionList { get; set; }
    }
}