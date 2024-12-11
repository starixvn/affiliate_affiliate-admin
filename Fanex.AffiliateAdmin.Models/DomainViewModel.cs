namespace Fanex.AffiliateAdmin.Models
{
    using Resources;
    using System.ComponentModel.DataAnnotations;

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