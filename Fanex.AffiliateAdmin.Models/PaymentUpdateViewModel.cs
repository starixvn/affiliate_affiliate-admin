namespace Fanex.AffiliateAdmin.Models
{
    using DTO;
    using Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PaymentUpdateViewModel
    {
        public PaymentDetail PaymentDetail { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrAdminRemark")]
        public string AdminRemark { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrAffRemark")]
        public string AffRemark { get; set; }

        [Required]
        public long TransId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ApprovedErr")]
        public string Status { get; set; }

        public bool FromStatement { get; set; }
    }
}