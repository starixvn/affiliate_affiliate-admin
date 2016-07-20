namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using Core.Configurations;
    using DTO;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerEditViewModel
    {
        [Required]
        public int BannerId { get; set; }

        [Display(Name = "File", ResourceType = typeof(Lang))]
        public HttpPostedFileBase File { get; set; }

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
        [RegularExpression(@"^([a-zA-Z0-9-_. ]+)$", ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "InvalidFileName")]
        [Display(Name = "filename", ResourceType = typeof(Lang))]
        public string FileName { get; set; }

        public IEnumerable<MediaType> SubCategories { get; set; }

        #region Option Lists

        public IEnumerable<SelectListItem> BannerCategoryOptionList
        {
            get
            {
                var optionList = new List<SelectListItem>();
                foreach (var banner in MediaSettings.BannerCategories)
                {
                    var text = Lang.ResourceManager.GetString(banner.Value.Replace(" ", string.Empty));

                    optionList.Add(new SelectListItem()
                    {
                        Value = banner.TypeId.ToString(),
                        Text = string.IsNullOrEmpty(text) ? banner.Value : text,
                        Selected = Category == banner.TypeId
                    });
                }

                return optionList; 
            }
        }

        public SelectList MemberSiteSupportedLanguageOptionList
        {
            get
            {
                return new SelectList(AppSettings.MemberSiteSiteSupportedLanguages, "LanguageID", "LanguageName", Language);
            }
        }

         public SelectList PlayerCurrencyOptionList
        {
            get
            {
                return new SelectList(AppSettings.PlayerCurrencies, "CurrencyID", "CurrencyCode", Currency.ToString());
            }
        }

        public SelectList AffiliateTypeOptionList
        {
            get
            {
                return new SelectList(AppSettings.AffTypes, "TypeID", "Name", AffType.ToString());
            }
        }

        public IEnumerable<SelectListItem> BannerTypeOptionList
        {
            get
            {
                var optionList = new List<SelectListItem>();

                foreach (var item in MediaSettings.BannerTypes)
                {
                    var text = Lang.ResourceManager.GetString(item.Value.Replace(" ", string.Empty));

                    optionList.Add(new SelectListItem
                    {
                        Value = item.TypeId.ToString(),
                        Text = string.IsNullOrEmpty(text) ? item.Value : text,
                        Selected = item.TypeId == ImageType
                    });
                }

                return optionList;
            }
        }

        public IEnumerable<SelectListItem> BannerSubCategoryOptionList
        {
            get
            {
                SubCategories = SubCategories ?? new List<MediaType>();

                var optionList = new List<SelectListItem>();

                foreach (var category in SubCategories)
                {
                    var text = Lang.ResourceManager.GetString(category.Value.Replace(" ", string.Empty));

                    optionList.Add(new SelectListItem()
                    {
                        Value = category.TypeId.ToString(),
                        Text = string.IsNullOrEmpty(text) ? category.Value : text,
                        Selected = category.TypeId == SubCategory
                    });
                }

                return optionList;
            }
        }

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

        #endregion
    }
}