// -----------------------------------------------------------------------
// <copyright file="AccountViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AccountViewModel
    {
        public int CustID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "ValidUsername")]
        [Display(ResourceType = typeof(Resources.Lang), Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "ErrPasswordRequired")]
        [Display(ResourceType = typeof(Resources.Lang), Name = "Password")]
        public string Password { get; set; }
    }
}
