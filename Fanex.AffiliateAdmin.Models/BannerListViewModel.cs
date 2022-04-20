namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerListViewModel : TextListViewModel
    {
        [Display(Name = "ImageType", ResourceType = typeof(Lang))]
        public short? ImageType { get; set; }

        [Display(Name = "Size", ResourceType = typeof(Lang))]
        public string Size { get; set; }

        [Display(Name = "AffType", ResourceType = typeof(Lang))]
        public short? AffType { get; set; }

        #region Option List

        public IEnumerable<SelectListItem> AffiliateTypeOptionList { get; set; }

        public IEnumerable<SelectListItem> BannerTypeOptionList { get; set; }

        public IEnumerable<SelectListItem> BannerSizeOptionList { get; set; }

        #endregion
    }
}