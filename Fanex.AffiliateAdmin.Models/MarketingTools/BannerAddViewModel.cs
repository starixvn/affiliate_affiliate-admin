namespace Fanex.AffiliateAdmin.Models.MarketingTools
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerAddViewModel : BannerInfoViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrFile")]
        [Display(Name = "File", ResourceType = typeof(Lang))]
        public override HttpPostedFileBase File { get; set; }
    }
}