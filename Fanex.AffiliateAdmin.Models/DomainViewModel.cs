namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;
    using Resources;

    /// <summary>
    /// Affiliate domain view model.
    /// </summary>
    public class DomainViewModel
    {
        public int DomainID { get; set; }

        [Display(Name = "Domain", ResourceType = typeof(Lang))]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "DomainIsRequired")]
        public string DomainName { get; set; }
    }
}