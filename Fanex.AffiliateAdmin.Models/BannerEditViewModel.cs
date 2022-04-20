namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerEditViewModel : BannerInfoViewModel
    {
        [Required]
        public int BannerId { get; set; }

        [Display(Name = "File", ResourceType = typeof(Lang))]
        public override HttpPostedFileBase File { get; set; }
    }
}