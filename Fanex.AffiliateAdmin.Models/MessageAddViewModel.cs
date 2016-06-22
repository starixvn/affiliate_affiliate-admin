namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageAddViewModel
    {
        [Display(Name = "Important", ResourceType = typeof(Lang))]
        public bool Important { get; set; }
    }
}