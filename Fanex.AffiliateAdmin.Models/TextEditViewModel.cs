namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TextEditViewModel : TextAddViewModel
    {
        [Required]
        public int BannerId { get; set; }
    }
}