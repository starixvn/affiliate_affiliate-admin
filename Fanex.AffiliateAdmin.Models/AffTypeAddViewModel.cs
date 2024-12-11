namespace Fanex.AffiliateAdmin.Models
{
    using Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffTypeAddViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "NameFieldIsRequired")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
