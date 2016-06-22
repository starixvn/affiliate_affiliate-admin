namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageDeleteViewModel
    {
        [Required]
        public string MsgIds { get; set; }
    }
}