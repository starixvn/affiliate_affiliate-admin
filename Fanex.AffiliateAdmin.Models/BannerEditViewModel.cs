// --------------------------------------------------------------------------
// <copyright file="BannerEditViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.30">Created</change>
// </history>

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

        public SelectList BannerCategoryOptionList
        {
            get
            {
                return new SelectList(MediaSettings.BannerCategories, "TypeId", "Value", Category.ToString());
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

        public SelectList BannerTypeOptionList
        {
            get
            {
                return new SelectList(MediaSettings.BannerTypes, "TypeId", "Value", ImageType.ToString());
            }
        }

        public SelectList BannerSubCategoryOptionList
        {
            get
            {
                SubCategories = SubCategories ?? new List<MediaType>();

                return new SelectList(SubCategories, "TypeId", "Value", SubCategory.HasValue ? SubCategory.Value.ToString() : string.Empty);
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