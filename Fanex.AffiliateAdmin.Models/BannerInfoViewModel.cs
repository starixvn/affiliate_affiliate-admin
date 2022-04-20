namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using DTO;
    using Resources;
    public abstract class BannerInfoViewModel
    {
        public abstract HttpPostedFileBase File { get; set; }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(Lang))]
        public short Category { get; set; }

        [Display(Name = "SubCategory", ResourceType = typeof(Lang))]
        public short? SubCategory { get; set; }

        [Required]
        [Display(Name = "AffType", ResourceType = typeof(Lang))]
        public short AffType { get; set; }

        [Required]
        [Display(Name = "Language", ResourceType = typeof(Lang))]
        public string Language { get; set; }

        [Required]
        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public short Currency { get; set; }

        [Required]
        [Display(Name = "ImageType", ResourceType = typeof(Lang))]
        public short ImageType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrmsgFileNameEmpty")]
        [RegularExpression(@"^([a-zA-Z0-9-_.]+)$", ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "InvalidFileName")]
        [Display(Name = "filename", ResourceType = typeof(Lang))]
        public string FileName { get; set; }

        public IEnumerable<MediaType> SubCategories { get; set; }

        public IEnumerable<SelectListItem> BannerCategoryOptionList { get; set; }

        public SelectList MemberSiteSupportedLanguageOptionList { get; set; }

        public SelectList PlayerCurrencyOptionList { get; set; }

        public SelectList AffiliateTypeOptionList { get; set; }

        public IEnumerable<SelectListItem> BannerTypeOptionList { get; set; }

        public IEnumerable<SelectListItem> BannerSubCategoryOptionList { get; set; }

        public short GetCategory
        {
            get
            {
                if (SubCategory != null)
                {
                    return SubCategory.Value;
                }

                return Category;
            }
        }
    }
}
