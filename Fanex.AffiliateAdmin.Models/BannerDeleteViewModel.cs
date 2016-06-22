namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerDeleteViewModel
    {
        [Required]
        public string BannerIds { get; set; }

        [Required]
        public string Contents { get; set; }
    }
}