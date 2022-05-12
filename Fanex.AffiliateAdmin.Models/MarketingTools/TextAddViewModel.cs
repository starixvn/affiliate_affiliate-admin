namespace Fanex.AffiliateAdmin.Models.MarketingTools
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TextAddViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "CategoryFieldIsRequired")]
        [Display(Name = "Category", ResourceType = typeof(Lang))]
        public short Category { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "TextBoxFieldIsRequired")]
        [Display(Name = "Text", ResourceType = typeof(Lang))]
        public string Content { get; set; }
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "CurrencyFieldIsRequired")]
        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public short Currency { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Lang))]
        public string Language { get; set; }
        #region Option List

        public IEnumerable<SelectListItem> BannerCategoryOptionList { get; set; }

        public IEnumerable<SelectListItem> MemberSiteSupportedLanguageOptionList { get; set; }

        public IEnumerable<SelectListItem> PlayerCurrencyOptionList { get; set; }

        #endregion
    }
}