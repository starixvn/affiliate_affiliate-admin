// -----------------------------------------------------------------------
// <copyright file="PasswordViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PasswordViewModel
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "ErrPasswordRequired")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Lang), Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "ConfirmPasswordIsRequired")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Lang), Name = "ConfirmPass")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "PassConfirm")]
        public string ConfirmPass { get; set; }
    }
}
