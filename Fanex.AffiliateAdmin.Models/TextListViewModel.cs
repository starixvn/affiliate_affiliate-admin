namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TextListViewModel
    {
        [Display(Name = "Category", ResourceType = typeof(Lang))]
        public short? Category { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public short? Currency { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Lang))]
        public string Language { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        #region Option List

        public IEnumerable<SelectListItem> BannerCategoryOptionList { get; set; }

        public IEnumerable<SelectListItem> MemberSiteSupportedLanguageOptionList { get; set; }

        public IEnumerable<SelectListItem> PlayerCurrencyOptionList { get; set; }

        #endregion
    }
}