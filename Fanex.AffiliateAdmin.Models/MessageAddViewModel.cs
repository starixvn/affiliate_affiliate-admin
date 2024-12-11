namespace Fanex.AffiliateAdmin.Models
{
    using Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageAddViewModel
    {
        [Display(Name = "Important", ResourceType = typeof(Lang))]
        public bool Important { get; set; }
    }
}